using System.Drawing;

namespace XLGraphicBot.services
{
	public interface IBitmapService
	{
		Bitmap CreateBitmap(string filename);
	}

	public class BitmapService : IBitmapService
	{
		public Bitmap CreateBitmap(string filename)
		{
			return new Bitmap(filename);
		}
	}
}
