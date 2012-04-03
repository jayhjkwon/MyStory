using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web.Routing;

namespace MyStory.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                Expression<Func<TModel, TValue>> expression,
                                                                object htmlAttributes)
        {
            return LabelFor(html, expression, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html,
                                                                Expression<Func<TModel, TValue>> expression,
                                                                IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();

            if (String.IsNullOrEmpty(labelText))
            {
                return MvcHtmlString.Empty;
            }

            TagBuilder tag = new TagBuilder("label");
            tag.MergeAttributes(htmlAttributes);
            tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tag.SetInnerText(labelText);

            return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        }

        public static HtmlString GetGravatarHtml(this HtmlHelper html, string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Microsoft.Web.Helpers.Gravatar.GetHtml(email, defaultImage:"monsterid");

            return Microsoft.Web.Helpers.Gravatar.GetHtml(email);
        }

        public static MvcHtmlString TagBadge(this HtmlHelper html, int rank, int tagCount, string tagText)
        {
            TagBuilder tagSpan = new TagBuilder("span");
            
            switch (rank)
            {
                case 1:
                    tagSpan.AddCssClass("badge-warning");
                    break;
                case 2:
                    tagSpan.AddCssClass("badge-success");
                    break;
                case 3 :
                    tagSpan.AddCssClass("badge-info");
                    break;
                default :
                    break;
            } 
            tagSpan.AddCssClass("badge");
            
            tagSpan.SetInnerText(tagCount.ToString());
            
            TagBuilder tagA = new TagBuilder("a");
            tagA.Attributes.Add("href", "#");
            tagA.InnerHtml = string.Format("{0} {1}", tagSpan.ToString(), html.Encode(tagText));

            TagBuilder tagLi = new TagBuilder("li");
            tagLi.InnerHtml = tagA.ToString();

            string a = tagLi.ToString(TagRenderMode.Normal);
            //return a;
            return MvcHtmlString.Create(tagLi.ToString(TagRenderMode.Normal));
        }
    }
}