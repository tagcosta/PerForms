#region License
// Copyright (c) 2010 Tiago Costa
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace PerForms.Util
{
    public class HeadLinksAndTemplates
    {
        public enum ELang
        {
            EN = 1,
            PT = 2
        }

        public ELang Lang { get; set; }

        public string CSSTemplate = "<link href='{0}' rel='stylesheet' type='text/css' />";
        public string JSTemplate = "<script src='{0}' type='text/javascript'></script>";
        
        public string CSSPath = "PerForms/css/";
        public string JSPath = "PerForms/js/";

        public string TemplateTemplate = "<script id='{0}' type='text/x-jquery-tmpl'>{1}</script>";

        #region Templates

        public string performs_fieldtemplate = @"
        <tr id='${FieldID}' class='performs-field ${FieldClass}'>
            {{tmpl '#performs_template_label'}}
            {{if FieldLabel == ''}}
                <td colspan='2'>
            {{else}}
                <td>
            {{/if}}
                <table cellspacing='0' class='performs-field-content'>
                    <tr>
                        <td>{{tmpl '#' + FieldTemplate}}</td>
                        <td class='performs-field-message'></td>
                    </tr>
                </table>
            </td>
        </tr>
        ";

        public string performs_template_label = @"
        {{if FieldLabel != ''}}
            <td class='performs-field-label' width='${LabelWidth}'>${FieldLabel}</td>
        {{/if}}
        ";

        public string performs_fieldtemplate_text = @"
        <input type='text' class='ui-state-default performs-field-fieldtext-input' />
        ";

        public string performs_fieldtemplate_textarea = @"
        <textarea class='ui-state-default performs-field-fieldtextarea-textarea'></textarea>
        ";

        public string performs_fieldtemplate_radios = @"
        {{each KeyValues}}
            <input type='radio' id='${FieldID}_${Key}' name='${FieldID}' value='${Key}' />
            <label for='${FieldID}_${Key}'>${Value}</label>
        {{/each}}
        ";

        public string performs_fieldtemplate_checkboxes = @"
        {{each KeyValues}}
            <input type='checkbox' id='${FieldID}_${Key}' value='${Key}' />
            <label for='${FieldID}_${Key}'>${Value}</label>
        {{/each}}
        ";

        public string performs_fieldtemplate_combobox = @"
        <input type='text' class='ui-state-default performs-field-fieldcombobox-input' />
        ";

        public string performs_fieldtemplate_button = @"
        <input type='button' value='${ButtonLabel}' />
        ";

        public string performs_fieldtemplate_multiselectbox = @"
        <select multiple='multiple' size='5' class='multiselect'>
        {{each KeyValues}}
            <option value='${Key}'>${Value}</option>
        {{/each}}
        </select>
        ";

        public string performs_fieldtemplate_gridview = @"
        <table id='${FieldID}-gridview'></table>
        <div id='${FieldID}-pager'></div>
        ";

        public string performs_fieldtemplate_label = @"
        <span></span>
        ";

        public string performs_fieldtemplate_date = @"
        <input type='text' class='ui-state-default performs-field-fielddate-input' />
        ";

        public string performs_fieldtemplate_upload = @"
        <div class='performs-field-fieldupload-upload'></div>
        ";

        public string performs_fieldtemplate_free = @"
        {{html Source}}
        ";

        #endregion

        public HeadLinksAndTemplates()
        {
            Lang = ELang.EN;
        }

        public HeadLinksAndTemplates(ELang lang)
        {
            Lang = lang;
        }


        public void AddPerFormsLinksAndTemplates(Page page)
        {
            AddCustomCssLink(page, "ui.multiselect.css");
            AddCustomCssLink(page, "ui.jqgrid.css");
            AddCustomCssLink(page, "fileuploader.css");
            AddCustomCssLink(page, "PerForms.css");

            AddCustomJsLink(page, "json2.min.js");
            AddCustomJsLink(page, "jquery-1.4.4.min.js");
            AddCustomJsLink(page, "jquery-ui-1.8.6.custom.min.js");
            AddCustomJsLink(page, "jquery.blockUI.js");
            AddCustomJsLink(page, "jquery.tmpl.min.js");
            AddCustomJsLink(page, "ui.multiselect.js");
            switch (Lang)
            {
                case ELang.PT:
                    AddCustomJsLink(page, "grid.locale-pt.js");
                    AddCustomJsLink(page, "PerForms-pt.js");
                    break;
                case ELang.EN:
                    AddCustomJsLink(page, "grid.locale-en.js");
                    AddCustomJsLink(page, "PerForms-en.js");
                    break;
            }
            AddCustomJsLink(page, "jquery.jqGrid.min.js");
            AddCustomJsLink(page, "fileuploader.js");
            AddCustomJsLink(page, "PerForms.js");

            BrowserDetector.EBrowser browser = new BrowserDetector().DetectBrowser();
            switch (browser)
            {
                case BrowserDetector.EBrowser.IE6:
                    AddCustomJsLink(page, "PerForms-ie6.js");
                    break;
                case BrowserDetector.EBrowser.IE7:
                    AddCustomJsLink(page, "PerForms-ie7.js");
                    break;
                case BrowserDetector.EBrowser.IE8:
                    AddCustomJsLink(page, "PerForms-ie8.js");
                    break;
                case BrowserDetector.EBrowser.Firefox:
                    AddCustomJsLink(page, "PerForms-firefox.js");
                    break;
                case BrowserDetector.EBrowser.Chrome:
                    AddCustomJsLink(page, "PerForms-chrome.js");
                    break;
                case BrowserDetector.EBrowser.Safari:
                    AddCustomJsLink(page, "PerForms-safari.js");
                    break;
                case BrowserDetector.EBrowser.Opera:
                    AddCustomJsLink(page, "PerForms-opera.js");
                    break;
            }

            AddCustomTemplate(page, "performs_fieldtemplate", performs_fieldtemplate);
            AddCustomTemplate(page, "performs_template_label", performs_template_label);
            AddCustomTemplate(page, "performs_fieldtemplate_text", performs_fieldtemplate_text);
            AddCustomTemplate(page, "performs_fieldtemplate_textarea", performs_fieldtemplate_textarea);
            AddCustomTemplate(page, "performs_fieldtemplate_radios", performs_fieldtemplate_radios);
            AddCustomTemplate(page, "performs_fieldtemplate_checkboxes", performs_fieldtemplate_checkboxes);
            AddCustomTemplate(page, "performs_fieldtemplate_combobox", performs_fieldtemplate_combobox);
            AddCustomTemplate(page, "performs_fieldtemplate_button", performs_fieldtemplate_button);
            AddCustomTemplate(page, "performs_fieldtemplate_multiselectbox", performs_fieldtemplate_multiselectbox);
            AddCustomTemplate(page, "performs_fieldtemplate_gridview", performs_fieldtemplate_gridview);
            AddCustomTemplate(page, "performs_fieldtemplate_label", performs_fieldtemplate_label);
            AddCustomTemplate(page, "performs_fieldtemplate_date", performs_fieldtemplate_date);
            AddCustomTemplate(page, "performs_fieldtemplate_upload", performs_fieldtemplate_upload);
            AddCustomTemplate(page, "performs_fieldtemplate_free", performs_fieldtemplate_free);
        }

        public void AddCustomCssLink(Page page, string cssFileName)
        {
            page.Header.Controls.Add(new LiteralControl(string.Format(CSSTemplate, CSSPath + cssFileName)));
        }

        public void AddCustomJsLink(Page page, string jsFileName)
        {
            page.Header.Controls.Add(new LiteralControl(string.Format(JSTemplate, JSPath + jsFileName)));
        }

        public void AddPerFormsLinksAndTemplates(UserControl userControl)
        {
            AddPerFormsLinksAndTemplates(userControl.Page);
        }

        public void AddCustomCssLink(UserControl userControl, string cssFileName)
        {
            AddCustomCssLink(userControl.Page, cssFileName);
        }

        public void AddCustomJsLink(UserControl userControl, string jsFileName)
        {
            AddCustomJsLink(userControl.Page, jsFileName);
        }

        public void AddCustomTemplate(Page page, string id, string content)
        {
            page.Header.Controls.Add(new LiteralControl(string.Format(TemplateTemplate, id, content)));
        }

        public void AddCustomTemplate(UserControl userControl, string id, string content)
        {
            AddCustomTemplate(userControl.Page, id, content);
        }
    }
}
