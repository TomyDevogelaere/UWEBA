using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;

namespace WhatIsNewInMVC.TagHelpers;
[HtmlTargetElement(tag:"ul", Attributes = ItemsAttributeName + "," + PropertyAttributeName)]
public class ListTagHelper: TagHelper
{
    public const string ItemsAttributeName = "u2u-items";
    public const string PropertyAttributeName = "u2u-property";

    [HtmlAttributeName(ItemsAttributeName)]
    public IEnumerable<object> Items { get; set; } = Enumerable.Empty<object>();
    [HtmlAttributeName(PropertyAttributeName)]
    public string Property { get; set; } = string.Empty;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (!Items.Any())
        {
            return;
        }

        Type elementType = Items.First().GetType();
        Func<object, object> lambdaExpression = CreateExpression(elementType, Property);
        foreach (object? item in Items)
        {
            string? result = lambdaExpression.Invoke(item).ToString();
            output.Content.AppendHtml($"<li>{result ?? string.Empty}</li>");
        }
        base.Process(context, output);
        Func<object,object> CreateExpression(Type type, string propertyName)
        {
            ParameterExpression lambdaParam = Expression.Parameter(typeof(object), "e");
            Expression body = Expression.Convert(lambdaParam, type);
            string[]? nestedProperties = propertyName.Split('.');
            foreach (string? member in nestedProperties)
            {
                body = Expression.PropertyOrField(body, member);
            }
            return Expression.Lambda<Func<object, object>>(
                body: body,
                parameters:lambdaParam).Compile();
        }
    }
}
