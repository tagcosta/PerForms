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

namespace PerForms.Util
{
    public class MIMEType
    {
        public string GetExtension(string fileName)
        {
            string extension = "";
            if (fileName.IndexOf('.') != -1)
            {
                int index = fileName.LastIndexOf('.');
                extension = fileName.Substring(index, fileName.Length - index);
                extension = extension.Trim('.');
            }
            return extension;
        }

        public string GetMIMEType(string extension)
        {
            switch (extension)
            {
                case "html":
                case "htm":
                    return "text/html";

                case "txt":
                case "c":
                case "c++":
                case "cc":
                case "h":
                    return "text/plain";

                case "talk":
                    return "text/x-speech";

                case "css":
                    return "text/css";

                case "gif":
                    return "image/gif";

                case "xbm":
                    return "image/x-xbitmap";

                case "xpm":
                    return "image/x-xpixmap";

                case "png":
                    return "image/x-png";

                case "ief":
                    return "image/ief";

                case "jpeg":
                case "jpg":
                case "jpe":
                    return "image/jpeg";

                case "tiff":
                case "tif":
                    return "image/tiff";

                case "rgb":
                    return "image/rgb";

                case "g3f":
                    return "image/g3fax";

                case "xwd":
                    return "image/x-xwindowdump";

                case "pict":
                    return "image/x-pict";

                case "ppm":
                    return "image/x-portable-pixmap";

                case "pgm":
                    return "image/x-portable-graymap";

                case "pbm":
                    return "image/x-portable-bitmap";

                case "pnm":
                    return "image/x-portable-anymap";

                case "bmp":
                    return "image/x-ms-bmp";

                case "ras":
                    return "image/x-cmu-raster";

                case "pcd":
                    return "image/x-photo-cd";

                case "cgm":
                    return "image/cgm";

                case "mil":
                case "cal":
                    return "image/x-cals";

                case "fif":
                    return "image/fif";

                case "dsf":
                    return "image/x-mgx-dsf";

                case "cmx":
                    return "image/x-cmx";

                case "wi":
                    return "image/wavelet";

                case "dwg":
                    return "image/vnd.dwg";

                case "dxf":
                    return "image/vnd.dxf";

                case "svf":
                    return "image/vnd.svf";

                case "au":
                case "snd":
                    return "audio/basic";

                case "aif":
                case "aiff":
                case "aifc":
                    return "audio/x-aiff";

                case "wav":
                    return "audio/x-wav";

                case "mpa":
                case "abs":
                case "mpega":
                    return "audio/x-mpeg";

                case "mp2a":
                case "mpa2":
                    return "audio/x-mpeg-2";

                case "es":
                    return "audio/echospeech";

                case "vox":
                    return "audio/voxware";

                case "lcc":
                    return "application/fastman";

                case "ra":
                case "ram":
                    return "application/x-pn-realaudio";

                case "mmid":
                    return "x-music/x-midi";

                case "skp":
                    return "application/vnd.koan";

                case "mpeg":
                case "mpg":
                case "mpe":
                    return "video/mpeg";

                case "mpv2":
                case "mp2v":
                    return "video/mpeg-2";

                case "qt":
                case "mov":
                    return "video/quicktime";

                case "avi":
                    return "video/x-msvideo";

                case "movie":
                    return "video/x-sgi-movie";

                case "vdo":
                    return "video/vdo";

                case "viv":
                    return "video/vnd.vivo";

                case "pac":
                    return "application/x-ns-proxy-autoconfig";

                case "ice":
                    return "x-conference/x-cooltalk";

                case "ai":
                case "eps":
                case "ps":
                    return "application/postscript";

                case "rtf":
                    return "application/rtf";

                case "pdf":
                    return "application/pdf";

                case "t":
                case "tr":
                case "roff":
                    return "application/x-troff";

                case "man":
                    return "application/x-troff-man";

                case "me":
                    return "application/x-troff-me";

                case "ms":
                    return "application/x-troff-ms";

                case "latex":
                    return "application/x-latex";

                case "tex":
                    return "application/x-tex";

                case "texinfo":
                case "texi":
                    return "application/x-texinfo";

                case "dvi":
                    return "application/x-dvi";

                case "oda":
                    return "application/oda";

                case "evy":
                    return "application/envoy";

                case "doc":
                case "docx":
                    return "application/msword";

                case "xls":
                case "xlsx":
                    return "application/vnd.ms-excel";

                case "fm":
                case "frame":
                    return "application/vnd.framemaker";

                case "gtar":
                    return "application/x-gtar";

                case "tar":
                    return "application/x-tar";

                case "ustar":
                    return "application/x-ustar";

                case "bcpio":
                    return "application/x-bcpio";

                case "cpio":
                    return "application/x-cpio";

                case "shar":
                    return "application/x-shar";

                case "zip":
                    return "application/zip";

                case "hqx":
                    return "application/mac-binhex40";

                case "sit":
                case "sea":
                    return "application/x-stuffit";

                case "bin":
                case "uu":
                    return "application/octet-stream";

                case "exe":
                    return "application/octet-stream";

                case "src":
                case "wsrc":
                    return "application/x-wais-source";

                case "hdf":
                    return "application/hdf";

                case "js":
                case "ls":
                case "mocha":
                    return "text/javascript";

                case "sh":
                    return "application/x-sh";

                case "csh":
                    return "application/x-csh";

                case "pl":
                    return "application/x-perl";

                case "tcl":
                    return "application/x-tcl";

                case "spl":
                    return "application/futuresplash";

                case "mbd":
                    return "application/mbedlet";

                case "rad":
                    return "application/x-rad-powermedia";

                case "ppz":
                    return "application/mspowerpoint";

                case "asp":
                    return "application/x-asap";

                case "asn":
                    return "application/astound";

                case "axs":
                    return "application/x-olescrip";

                case "ods":
                    return "application/x-oleobject";

                case "opp":
                    return "x-form/x-openscape";

                case "wba":
                    return "application/x-webbasic";

                case "frm":
                    return "application/x-alpha-form";

                case "wfx":
                    return "x-script/x-wfxclient";

                case "pcn":
                    return "application/x-pcn";

                case "ppt":
                    return "application/vnd.ms-powerpoint";

                case "svd":
                    return "application/vnd.svd";

                case "ins":
                    return "application/x-net-install";

                case "ccv":
                    return "application/ccv";

                case "vts":
                    return "workbook/formulaone";

                case "wrl":
                case "vrml":
                    return "model/vrml";

                case "vrw":
                    return "x-world/x-vream";

                case "p3d":
                    return "application/x-p3d";

                case "svr":
                    return "x-world/x-svr";

                case "wvr":
                    return "x-world/x-wvr";

                case "3dmf":
                    return "x-world/x-3dmf";

                case "ma":
                    return "application/mathematica";

                case "msh":
                    return "model/mesh";

                case "v5d":
                    return "application/vis5d";

                case "igs":
                    return "application/iges";

                case "dwf":
                    return "drawing/x-dwf";

                case "showcase":
                case "slides":
                case "sc":
                case "sho":
                case "show":
                    return "application/x-showcase";

                case "insight":
                    return "application/x-insight";

                case "ano":
                    return "application/x-annotator";

                case "dir":
                    return "application/x-dirview";

                case "lic":
                    return "application/x-enterlicense";

                case "faxmgr":
                    return "application/x-fax-manager";

                case "faxmgrjob":
                    return "application/x-fax-manager-job";

                case "icnbk":
                    return "application/x-iconbook";

                case "wb":
                    return "application/x-inpview";

                case "inst":
                    return "application/x-install";

                case "mail":
                    return "application/x-mailfolder";

                case "pp":
                case "ppages":
                    return "application/x-ppages";

                case "sgi-lpr":
                    return "application/x-sgi-lpr";

                case "tardist":
                    return "application/x-tardist";

                case "ztardist":
                    return "application/x-ztardist";

                case "wkz":
                    return "application/x-wingz";

                case "iv":
                    return "graphics/x-inventor";

                case "msg":
                    return "application/vnd.ms-outlook";

                default:
                    return String.Empty;

            }
        }
    }
}
