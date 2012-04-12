using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hd.Web.Extensions
{
	public interface IRichEditControl : ITextControl
	{
		string EditorClientID { get; }
		Unit Height { get; set; }
		Unit Width { get; set; }
		int MaxLength { get; set; }
		RichMode Mode { set; }
	}

	public enum RichMode
	{
		Simple,
		Full,
		None,
		SimpleWithTable
	}
}
