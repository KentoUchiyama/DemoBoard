using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoBoard.Models
{
	public class PostViewModel
	{

		public List<PostModel> postList { get; set; }

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public PostViewModel()
		{
			postList = new List<PostModel>();
		}
	}
}
