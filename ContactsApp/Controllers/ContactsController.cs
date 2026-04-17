using ContactsApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContactsController(AppDbContext context) => _context = context;

        // contacts list view - available to everyone
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetContacts()
        {
            return await _context.Contacts
                .Select(c => new { c.Id, c.FirstName, c.LastName })
                .ToListAsync();
        }

        // contact details - requires logging in
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null) return NotFound();
            return contact;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact(Contact contact)
        {
            // check if email unique
            if (_context.Contacts.Any(c => c.Email == contact.Email))
            {
                // return json
                return BadRequest(new { message = "Email must be unique." });
            }

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
        }

        // deleting contact - requires login
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null) return NotFound();
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // updating contact - requires login
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(int id, [FromBody] Contact contact)
        {
            // does id in url match contact
            if (id != contact.Id)
            {
                return BadRequest("Id of contact doesn't match.");
            }

            // info for db that entry was edited
            _context.Entry(contact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Contacts.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // successful edit
        }

        // dictionary for frontend
        [HttpGet("dictionary")]
        public async Task<IActionResult> GetDictionary() => Ok(await _context.Dictionaries.ToListAsync());
    }
}
