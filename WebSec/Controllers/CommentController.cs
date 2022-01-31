#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebSec;
using WebSec.Models;

namespace WebSec.Controllers
{
    public class CommentController : Controller
    {
        private readonly SqlContext _context;
        private string[] _tags = new string[] { "<b>", "</b>", "<i>", "</i>" };

        public CommentController(SqlContext context)
        {
            _context = context;
        }

        // GET: Comment
        public async Task<IActionResult> Index()
        {
            return View(await _context.Messages.ToListAsync());
        }

        // GET: Comment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentEntity = await _context.Messages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentEntity == null)
            {
                return NotFound();
            }

            return View(commentEntity);
        }

        // GET: Comment/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Comment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Content")] CommentEntity commentEntity)
        {
            if (ModelState.IsValid)
            {
                string encodedContent = HttpUtility.HtmlEncode(commentEntity.Content);
                foreach(var tag in _tags)
                {
                    var encodedTag = HttpUtility.HtmlEncode(tag);
                    encodedContent = encodedContent.Replace(encodedTag, tag);
                }

                commentEntity.Content = encodedContent;

                _context.Add(commentEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(commentEntity);
        }

        // GET: Comment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentEntity = await _context.Messages.FindAsync(id);
            if (commentEntity == null)
            {
                return NotFound();
            }
            return View(commentEntity);
        }

        // POST: Comment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Content")] CommentEntity commentEntity)
        {
            if (id != commentEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commentEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentEntityExists(commentEntity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(commentEntity);
        }

        // GET: Comment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentEntity = await _context.Messages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentEntity == null)
            {
                return NotFound();
            }

            return View(commentEntity);
        }

        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commentEntity = await _context.Messages.FindAsync(id);
            _context.Messages.Remove(commentEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentEntityExists(int id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }
    }
}
