﻿using DryIoc;
using DryIocAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using ZKWeb.Core;
using ZKWeb.Plugins.Common.Base.src.HtmlBuilder;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Base.src.FormFieldHandlers {
	/// <summary>
	/// 勾选框分组
	/// 显示结构
	/// ▢ 全选
	/// 　▢ 多选框A ▢ 多选框B ▢ 多选框C
	/// </summary>
	[ExportMany(ContractKey = typeof(CheckBoxGroupFieldAttribute)), SingletonReuse]
	public class CheckBoxGroup : IFormFieldHandler {
		/// <summary>
		/// 获取表单字段的html
		/// </summary>
		public string Build(FormField field, Dictionary<string, string> htmlAttributes) {
			var provider = Application.Ioc.Resolve<FormHtmlProvider>();
			var attribute = (CheckBoxGroupFieldAttribute)field.Attribute;
			var listItemProvider = (IListItemProvider)Activator.CreateInstance(attribute.Source);
			var listItems = listItemProvider.GetItems().ToList();
			var templateManager = Application.Ioc.Resolve<TemplateManager>();
			var fieldHtml = new HtmlTextWriter(new StringWriter());
			fieldHtml.AddAttribute("name", field.Attribute.Name);
			fieldHtml.AddAttribute("value", JsonConvert.SerializeObject(field.Value ?? new string[0]));
			fieldHtml.AddAttribute("type", "hidden");
			fieldHtml.AddAttributes(provider.FormControlAttributes);
			fieldHtml.AddAttributes(htmlAttributes);
			fieldHtml.RenderBeginTag("input");
			fieldHtml.RenderEndTag();
			var html = templateManager.RenderTemplate("common.base/tmpl.checkbox_group.html", new {
				items = listItems,
				fieldName = field.Attribute.Name,
				fieldHtml = new HtmlString(fieldHtml.InnerWriter.ToString())
			});
			return provider.FormGroupHtml(field, htmlAttributes, html);
		}

		/// <summary>
		/// 解析提交的字段的值
		/// </summary>
		public object Parse(FormField field, string value) {
			return JsonConvert.DeserializeObject<IList<string>>(value);
		}
	}
}
