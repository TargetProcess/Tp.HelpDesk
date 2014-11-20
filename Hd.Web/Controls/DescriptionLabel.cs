using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hd.Web.Controls
{
	public class DescriptionLabel : Label
	{
		protected override void Render(HtmlTextWriter writer)
		{
			Text = VirtualizeImgMappedPaths(Text);
			base.Render(writer);
		}

		protected static string VirtualizeImgMappedPaths(string input)
		{
			try
			{
				if (String.IsNullOrEmpty(input))
					return input;

				return Regex.Replace(input, @"(<img[^>]*src=['""])((?:&#126;&#47;|~/)Attachment\.aspx)([^""']*?['""][^>]*?>)",
				                     "$1" + VirtualPathUtility.ToAbsolute("~/Attachment.ashx") + "$3",
				                     RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
			}
			catch (HttpException)
			{
				return input;
			}
		}
	}
}