using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SchoolDb>(opt => opt.UseInMemoryDatabase("SchoolList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/", () => "Creating a minimal API...");

app.MapGet("/students", async (SchoolDb db) => // Get all students
    await db.Students.ToListAsync());

app.MapGet("/subjects", async (SchoolDb db) => // Get all subjects
    await db.Subjects.ToListAsync());

app.MapGet("/students/active", async (SchoolDb db) => // Get all active students
    await db.Students.Where(s => s.Active == true).ToListAsync());

app.MapGet("/subjects/uv", async (SchoolDb db) => // Get all subject with UV >= 4
    await db.Subjects.Where(s => s.UV >= 4).ToListAsync());

app.MapGet("/students/{id}", async (int id, SchoolDb db) => // Get an especific student
    {
        var student = await db.Students.FindAsync(id);
        return student != null ? Results.Ok(student) : Results.NotFound();
    }
);

app.MapGet("/subjects/{id}", async (int id, SchoolDb db) => // Get an especific subject
    {
        var subject = await db.Subjects.FindAsync(id);
        return subject != null ? Results.Ok(subject) : Results.NotFound(); 
    }
);

app.MapPost("/students", async (Student student, SchoolDb db) => // Add a student
    {
        db.Students.Add(student);
        await db.SaveChangesAsync();

        return Results.Ok();
    }
);

app.MapPost("/subjects", async (Subject subject, SchoolDb db) => // Add a subject
    {
        db.Subjects.Add(subject);
        await db.SaveChangesAsync();

        return Results.Ok();
    }
);

app.MapPut("/students/{id}", async (int id, Student inputStudent, SchoolDb db) => // Change a student
{
    var student = await db.Students.FindAsync(id);

    if (student is null) return Results.NotFound();

    student.Name = inputStudent.Name;
    student.Active = inputStudent.Active;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapPut("/subjects/{id}", async (int id, Subject inputSubject, SchoolDb db) => // Change a subject
{
    var subject = await db.Subjects.FindAsync(id);

    if (subject is null) return Results.NotFound();

    subject.Title = inputSubject.Title;
    subject.UV = inputSubject.UV;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/students/{id}", async (int id, SchoolDb db) => // Delete a student
{
    if (await db.Students.FindAsync(id) is Student student)
    {
        db.Students.Remove(student);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.MapDelete("/subjects/{id}", async (int id, SchoolDb db) => // Delete a subject
{
    if (await db.Subjects.FindAsync(id) is Subject subject)
    {
        db.Subjects.Remove(subject);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();