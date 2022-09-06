namespace ConfigLab.Controllers;

public class PeopleController : Controller
{
  private readonly PeopleContext _context;
  private readonly Features _features;


  public PeopleController(PeopleContext context, Features features)
    {   _context = context;
        _features = features;
    }

  // GET: People
  public async Task<IActionResult> Index()
  {
    var peopleContext = this._context.People.Include(p => p.Address);
        if (_features.IndexV2)
        {
            return View(viewName: "IndexV2", model: await peopleContext.ToListAsync());
        }
        else
        {
            return View(await peopleContext.ToListAsync());
        }
    
  }

  // GET: People/Details/5
  public async Task<IActionResult> Details(int? id)
  {
    if (id == null)
    {
      return NotFound();
    }

    Person? person = await this._context.People
                                        .Include(p => p.Address)
                                        .FirstOrDefaultAsync(m => m.Id == id);
    if (person is null)
    {
      return NotFound();
    }

    return View(person);
  }

  // GET: People/Create
  public IActionResult Create()
  {
    ViewData["AddressId"] = new SelectList(this._context.Addresses, "Id", "Id");
    return View();
  }

  // POST: People/Create
  // To protect from overposting attacks, enable the specific properties you want to bind to.
  // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,AddressId")] Person person)
  {
    if (ModelState.IsValid)
    {
      this._context.Add(person);
      await this._context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }
    ViewData["AddressId"] = new SelectList(this._context.Addresses, "Id", "Id", person.AddressId);
    return View(person);
  }

  // GET: People/Edit/5
  public async Task<IActionResult> Edit(int? id)
  {
    if (id == null)
    {
      return NotFound();
    }

    Person? person = await this._context.People.FindAsync(id);
    if (person is null)
    {
      return NotFound();
    }
    ViewData["AddressId"] = new SelectList(this._context.Addresses, "Id", "Id", person.AddressId);
    return View(person);
  }

  // POST: People/Edit/5
  // To protect from overposting attacks, enable the specific properties you want to bind to.
  // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,AddressId")] Person person)
  {
    if (id != person.Id)
    {
      return NotFound();
    }

    if (ModelState.IsValid)
    {
      try
      {
        this._context.Update(person);
        await this._context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!PersonExists(person.Id))
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
    ViewData["AddressId"] = new SelectList(this._context.Addresses, "Id", "Id", person.AddressId);
    return View(person);
  }

  // GET: People/Delete/5
  public async Task<IActionResult> Delete(int? id)
  {
    if (id == null)
    {
      return NotFound();
    }

    Person? person = await this._context.People
                                        .Include(p => p.Address)
                                        .FirstOrDefaultAsync(m => m.Id == id);
    if (person is null)
    {
      return NotFound();
    }

    return View(person);
  }

  // POST: People/Delete/5
  [HttpPost, ActionName("Delete")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> DeleteConfirmed(int id)
  {
    Person? person = await this._context.People.FindAsync(id);
    if (person is not null)
    {
      this._context.People.Remove(person);
      await this._context.SaveChangesAsync();
    }
    return RedirectToAction(nameof(Index));
  }

  private bool PersonExists(int id) 
    => this._context.People.Any(e => e.Id == id);
}
