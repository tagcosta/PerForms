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

namespace PerForms.Actions
{
    [Serializable]
    public class ShowMessageAction : AJAXAction
    {
        /// <summary>
        /// <para>Allowed types of messages.</para>
        /// </summary>
        public enum EMessageType
        {
            ERROR = 0,
            HIGHLIGHT = 1
        }

        /// <summary>
        /// <para>The type of the message to show.</para>
        /// <para>The type only affects the appearance of the message.</para>
        /// </summary>
        public EMessageType MessageType { get; set; }

        /// <summary>
        /// <para>The content of the message.</para>
        /// <para>You can use html.</para>
        /// </summary>
        public string MessageContent { get; set; }

        /// <summary>
        /// <para>The title of the message.</para>
        /// <para>Renders in the header of the message.</para>
        /// <para>If empty, the message will still have a header but with no text on it.</para>
        /// </summary>
        public string MessageTitle { get; set; }

        /// <summary>
        /// <para>The subtitle of the message. You can leave this empty.</para>
        /// <para>Renders in the form of '<b>subtitle:</b> content'.</para>
        /// </summary>
        public string MessageSubtitle { get; set; }

        public ShowMessageAction(EMessageType type, string title, string subtitle, string content)
            : base(EAJAXActionType.ShowMessage)
        {
            MessageType = type;
            MessageTitle = title;
            MessageSubtitle = subtitle;
            MessageContent = content;
        }

        public ShowMessageAction(string title, string content) : this(EMessageType.HIGHLIGHT, title, "", content) { }
    }
}