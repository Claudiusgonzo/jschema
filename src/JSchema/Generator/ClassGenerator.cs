﻿// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Microsoft.JSchema.Generator
{
    /// <summary>
    /// Generate the text of a class.
    /// </summary>
    /// <remarks>
    /// Hat tip: Mike Bennett, "Generating Code with Roslyn",
    /// https://dogschasingsquirrels.com/2014/07/16/generating-code-with-roslyn/
    /// </remarks>
    public class ClassGenerator : ClassOrInterfaceGenerator
    {
        // Name used for the parameters of Equals methods.
        private const string OtherParameter = "other";

        // Name of the Count property of IList.
        private const string CountProperty = "Count";

        private readonly string _baseInterfaceName;

        // Value used to construct unique names for each of the loop variables
        // used in the implementation of the Equals method.
        private int _indexVarCount = 0;

        public ClassGenerator(JsonSchema rootSchema, string interfaceName, HintDictionary hintDictionary)
            : base(rootSchema, hintDictionary)
        {
            _baseInterfaceName = interfaceName;
        }

        public override BaseTypeDeclarationSyntax CreateTypeDeclaration(JsonSchema schema)
        {
            var classDeclaration = SyntaxFactory.ClassDeclaration(TypeName)
                .WithModifiers(
                    SyntaxFactory.TokenList(
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.PartialKeyword)));

            var baseTypes = new List<BaseTypeSyntax>();

            // If this class implements an interface, add the interface to
            // the base type list.
            if (_baseInterfaceName != null)
            {
                SimpleBaseTypeSyntax interfaceType =
                    SyntaxFactory.SimpleBaseType(
                        SyntaxFactory.ParseTypeName(_baseInterfaceName));

                baseTypes.Add(interfaceType);
            }

            var iEquatable = SyntaxFactory.SimpleBaseType(
                SyntaxFactory.GenericName(
                    SyntaxFactory.Identifier("IEquatable"),
                    SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(
                        new TypeSyntax[] {
                        SyntaxFactory.ParseTypeName(TypeName)
                        }))));

            baseTypes.Add(iEquatable);

            AddUsing("System"); // For IEquatable<T>

            if (baseTypes.Count > 0)
            {
                SeparatedSyntaxList<BaseTypeSyntax> separatedBaseList = SyntaxFactory.SeparatedList(baseTypes);
                BaseListSyntax baseList = SyntaxFactory.BaseList(separatedBaseList);
                classDeclaration = classDeclaration.WithBaseList(baseList);
            }

            return classDeclaration;
        }

        public override void AddMembers(JsonSchema schema)
        {
            List<MemberDeclarationSyntax> members = CreateProperties(schema);

            members.AddRange(new MemberDeclarationSyntax[]
                {
                OverrideObjectEquals(),
                ImplementIEquatableEquals(schema)
                });

            SyntaxList<MemberDeclarationSyntax> memberList = SyntaxFactory.List(members);

            TypeDeclaration = (TypeDeclaration as ClassDeclarationSyntax).WithMembers(memberList);
        }

        private MemberDeclarationSyntax OverrideObjectEquals()
        {
            return SyntaxFactory.MethodDeclaration(
                SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword)),
                "Equals")
                .WithModifiers(
                    SyntaxFactory.TokenList(
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.OverrideKeyword)))
                .WithParameterList(
                    SyntaxFactory.ParameterList(
                        SyntaxFactory.SingletonSeparatedList(
                            SyntaxFactory.Parameter(
                                default(SyntaxList<AttributeListSyntax>),
                                default(SyntaxTokenList), // modifiers
                                SyntaxFactory.PredefinedType(
                                    SyntaxFactory.Token(SyntaxKind.ObjectKeyword)),
                                SyntaxFactory.Identifier(OtherParameter),
                                default(EqualsValueClauseSyntax)))))
                .WithBody(
                    SyntaxFactory.Block(
                        SyntaxFactory.ReturnStatement(
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.IdentifierName("Equals"),
                                ArgumentList(
                                    SyntaxFactory.BinaryExpression(
                                        SyntaxKind.AsExpression,
                                        SyntaxFactory.IdentifierName(OtherParameter),
                                        SyntaxFactory.ParseTypeName(TypeName)))))));

        }

        private MemberDeclarationSyntax ImplementIEquatableEquals(JsonSchema schema)
        {
            return SyntaxFactory.MethodDeclaration(
                SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword)), "Equals")
                .WithModifiers(
                    SyntaxFactory.TokenList(
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                .WithParameterList(
                    SyntaxFactory.ParameterList(
                        SyntaxFactory.SingletonSeparatedList(
                            SyntaxFactory.Parameter(
                                default(SyntaxList<AttributeListSyntax>),
                                default(SyntaxTokenList), // modifiers
                                SyntaxFactory.ParseTypeName(TypeName),
                                SyntaxFactory.Identifier(OtherParameter),
                                default(EqualsValueClauseSyntax))
                        )))
                .WithBody(
                    SyntaxFactory.Block(MakeEqualityTests(schema)));
        }

        private StatementSyntax[] MakeEqualityTests(JsonSchema schema)
        {
            var statements = new List<StatementSyntax>();

            statements.Add(
                SyntaxFactory.IfStatement(
                    SyntaxFactory.BinaryExpression(
                        SyntaxKind.EqualsExpression,
                        SyntaxFactory.IdentifierName(OtherParameter),
                        NullLiteralExpression()),
                    SyntaxFactory.Block(Return(false))));

            if (schema.Properties != null)
            {
                foreach (var property in schema.Properties)
                {
                    string propName = property.Key.ToPascalCase();
                    switch (PropertyComparisonTypeDictionary[propName.ToCamelCase()])
                    {
                        case ComparisonType.OperatorEquals:
                            statements.Add(MakeOperatorEqualsTest(propName));
                            break;

                        case ComparisonType.ObjectEquals:
                            statements.Add(
                                MakeObjectEqualsTest(SyntaxFactory.IdentifierName(propName), OtherPropName(propName)));
                            break;

                        case ComparisonType.Collection:
                            statements.Add(MakeCollectionEqualsTest(propName));
                            break;

                        default:
                            break;
                    }
                }
            }

            // All comparisons succeeded.
            statements.Add(Return(true));

            return statements.ToArray();
        }

        private StatementSyntax MakeOperatorEqualsTest(string propName)
        {
            return SyntaxFactory.IfStatement(
                SyntaxFactory.BinaryExpression(
                    SyntaxKind.NotEqualsExpression,
                    SyntaxFactory.IdentifierName(propName),
                    OtherPropName(propName)),
                SyntaxFactory.Block(Return(false)));
        }

        private StatementSyntax MakeObjectEqualsTest(ExpressionSyntax left, ExpressionSyntax right)
        {
            return SyntaxFactory.IfStatement(
                // if (!(Object.Equals(Prop, other.Prop))
                SyntaxFactory.PrefixUnaryExpression(
                    SyntaxKind.LogicalNotExpression,
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("Object"),
                            SyntaxFactory.IdentifierName("Equals")),
                        ArgumentList(left, right))),
                SyntaxFactory.Block(Return(false)));
        }

        private StatementSyntax MakeCollectionEqualsTest(string propName)
        {
            return SyntaxFactory.IfStatement(
                // if (!Object.ReferenceEquals(Prop, other.Prop))
                SyntaxFactory.PrefixUnaryExpression(
                    SyntaxKind.LogicalNotExpression,
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName("Object"),
                                SyntaxFactory.IdentifierName("ReferenceEquals")),
                            ArgumentList(SyntaxFactory.IdentifierName(propName), OtherPropName(propName)))),
                SyntaxFactory.Block(
                    // if (Prop == null || other.Prop == null)
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.LogicalOrExpression,
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.EqualsExpression,
                                SyntaxFactory.IdentifierName(propName),
                                NullLiteralExpression()),
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.EqualsExpression,
                                OtherPropName(propName),
                                NullLiteralExpression())),
                        SyntaxFactory.Block(Return(false))),

                    // if (Prop.Count != other.Prop.Count)
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryExpression(
                            SyntaxKind.NotEqualsExpression,
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(propName),
                                SyntaxFactory.IdentifierName(CountProperty)),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                OtherPropName(propName),
                                SyntaxFactory.IdentifierName(CountProperty))),
                        SyntaxFactory.Block(Return(false))),

                    CollectionIndexLoop(propName, _indexVarCount++)
                    ));
        }

        private ForStatementSyntax CollectionIndexLoop(string propName, int indexVarCount)
        {
            string indexVarName = "i" + indexVarCount;

            return SyntaxFactory.ForStatement(
                SyntaxFactory.VariableDeclaration(
                    SyntaxFactory.ParseTypeName("int"),
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.VariableDeclarator(
                            SyntaxFactory.Identifier(indexVarName),
                            default(BracketedArgumentListSyntax),
                            SyntaxFactory.EqualsValueClause(
                                SyntaxFactory.LiteralExpression(
                                    SyntaxKind.NumericLiteralExpression,
                                    SyntaxFactory.Literal(0)))))),
                SyntaxFactory.SeparatedList<ExpressionSyntax>(),
                SyntaxFactory.BinaryExpression(
                    SyntaxKind.LessThanExpression,
                    SyntaxFactory.IdentifierName(indexVarName),
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName(propName),
                        SyntaxFactory.IdentifierName(CountProperty))),
                SyntaxFactory.SingletonSeparatedList<ExpressionSyntax>(
                    SyntaxFactory.PrefixUnaryExpression(
                        SyntaxKind.PreIncrementExpression,
                        SyntaxFactory.IdentifierName(indexVarName))),
                SyntaxFactory.Block(
                    SyntaxFactory.IfStatement(
                        SyntaxFactory.PrefixUnaryExpression(
                            SyntaxKind.LogicalNotExpression,
                            SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(
                                    SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("Object"),
                                    SyntaxFactory.IdentifierName("Equals")),
                                ArgumentList(
                                    SyntaxFactory.ElementAccessExpression(
                                        SyntaxFactory.IdentifierName(propName),
                                        BracketedArgumentList(
                                            SyntaxFactory.IdentifierName(indexVarName))),
                                    SyntaxFactory.ElementAccessExpression(
                                        OtherPropName(propName),
                                        BracketedArgumentList(
                                            SyntaxFactory.IdentifierName(indexVarName)))))),
                        SyntaxFactory.Block(Return(false)))));
        }

        #region Syntax helpers

        protected override SyntaxTokenList CreatePropertyModifiers()
        {
            var modifiers = SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword));
            if (_baseInterfaceName != null)
            {
                modifiers = modifiers.Add(SyntaxFactory.Token(SyntaxKind.OverrideKeyword));
            }

            return modifiers;
        }

        private ArgumentListSyntax ArgumentList(params ExpressionSyntax[] args)
        {
            return SyntaxFactory.ArgumentList(
                        SyntaxFactory.SeparatedList(
                            args.Select(arg => SyntaxFactory.Argument(arg))));

        }

        private  BracketedArgumentListSyntax BracketedArgumentList(params ExpressionSyntax[] args)
        {
            return SyntaxFactory.BracketedArgumentList(
                        SyntaxFactory.SeparatedList(
                            args.Select(arg => SyntaxFactory.Argument(arg))));
        }

        private ExpressionSyntax OtherPropName(string propName)
        {
            return SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.IdentifierName(OtherParameter),
                SyntaxFactory.IdentifierName(propName));
        }

        private ExpressionSyntax NullLiteralExpression()
        {
            return SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression);
        }

        private StatementSyntax Return(bool value)
        {
            return SyntaxFactory.ReturnStatement(
                SyntaxFactory.LiteralExpression(
                    value
                    ? SyntaxKind.TrueLiteralExpression
                    : SyntaxKind.FalseLiteralExpression));
        }

        #endregion Syntax helpers
    }
}
