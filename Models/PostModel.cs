using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DemoBoard.Models
{
	/// <summary>
	/// 投稿内容
	/// </summary>
	public class PostModel
	{
		/// <summary>
		/// データベースの主キー
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// 名前
		/// </summary>
		public string name { get; set; }
		/// <summary>
		/// 投稿日時
		/// </summary>
		public DateTime postDate { get; set; }
		/// <summary>
		/// 内容
		/// </summary>
		public string text { get; set; }
	}
}
