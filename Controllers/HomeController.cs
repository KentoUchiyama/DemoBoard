using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DemoBoard.Data;
using DemoBoard.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoBoard.Controllers
{
	public class HomeController : Controller
	{

		/// <summary>
		/// DBコンテキスト
		/// </summary>
		private readonly PostContext _context;

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="context"></param>
		public HomeController( PostContext context )
		{
			_context = context;
		}

		/// <summary>
		/// 表示
		/// </summary>
		/// <returns></returns>
		public async Task<IActionResult> Index()
		{
			// データベースからすべてのレコードを取得するクエリを定義
			var posts = from m in _context.Post
						select m;

			var postVM = new PostViewModel
			{
				// postsに定義されたクエリの実行結果をListに格納
				postList = await posts.ToListAsync()
			};

			return View( postVM );
		}

		/// <summary>
		/// 投稿
		/// </summary>
		/// <param name="name">名前</param>
		/// <param name="text">内容</param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> Post( string name, string text)
		{
			// NULL対策
			if ( name == null ) name = "名無し";
			if ( text == null ) text = "";

			// 投稿内容作成
			PostModel post = new PostModel();
			post.name = name;
			post.postDate = DateTime.Now;
			// Viewに渡したときに改行が反映されるように、\r\nを<br>に変換する
			post.text = text.Replace( "\r\n", "<br>" );

			// 投稿内容をDBコンテキストに追加
			_context.Add( post );
			await _context.SaveChangesAsync();

			// Indexにリダイレクト
			return RedirectToAction( nameof( Index ) );
		}

		/// <summary>
		/// 編集画面表示
		/// </summary>
		/// <param name="id">主キー</param>
		/// <returns></returns>
		public async Task<IActionResult> Edit( int? id )
		{
			if ( id == null )
			{
				return NotFound();
			}

			var post = await _context.Post.FindAsync( id );
			if ( post == null )
			{
				return NotFound();
			}
			return View( post );
		}

		/// <summary>
		/// 編集内容反映
		/// </summary>
		/// <param name="id">主キー</param>
		/// <param name="post">投稿データ</param>
		/// <returns></returns>
		[HttpPost]
		public async Task<IActionResult> Edit( int id, [Bind( "Id,name,postDate,text" )] PostModel post )
		{
			if ( id != post.Id )
			{
				return NotFound();
			}

			if ( ModelState.IsValid )
			{
				try
				{
					if ( post.text == null ) post.text = "";
					if ( post.text.Contains( "（編集済み）" ) == false )	post.text += "（編集済み）";
					_context.Update( post );
					await _context.SaveChangesAsync();
				}
				catch ( DbUpdateConcurrencyException )
				{
					if ( !PostExists( post.Id ) )
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction( nameof( Index ) );
			}
			return View( post );
		}

		/// <summary>
		/// 削除画面表示
		/// </summary>
		/// <param name="id">主キー</param>
		/// <returns></returns>
		public async Task<IActionResult> Delete( int? id )
		{
			if ( id == null )
			{
				return NotFound();
			}
			
			var posts = await _context.Post
				.FirstOrDefaultAsync( m => m.Id == id );
			if ( posts == null )
			{
				return NotFound();
			}

			return View( posts );
		}

		/// <summary>
		/// 削除処理
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpPost, ActionName( "Delete" )]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed( int id )
		{
			var post = await _context.Post.FindAsync( id );
			_context.Post.Remove( post );
			await _context.SaveChangesAsync();
			return RedirectToAction( nameof( Index ) );
		}

		private bool PostExists( int id )
		{
			return _context.Post.Any( e => e.Id == id );
		}

		[ResponseCache( Duration = 0, Location = ResponseCacheLocation.None, NoStore = true )]
		public IActionResult Error()
		{
			return View( new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier } );
		}
	}
}
