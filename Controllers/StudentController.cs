using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentNewApi.Models;
namespace StudentNewApi.Controllers;
[ApiController]
[Route("[controller]")]
// public class StudentController:ControllerBase{
//     private static List<Student> Students=new List<Student>(){
//             new Student(){
//                 Id=1,
//                 Name="Souvik",
//                 Age=22,
//                 city="Kolkata",
//                 subject="MCA"
//             }
//         };
//     [HttpGet]
//     [Route("getStudents")]
//     public async Task<ActionResult<Student>> GetStudents(){

//         return Ok(Students);
//     }
//     [HttpGet]
//     [Route("getStudent")]
//     public async Task<ActionResult<Student>> GetStudent(int id){
//         var student = Students.Find(x=>x.Id==id);
//         if(student==null)
//             return BadRequest("No student found!");
//         return Ok(student);
//     }
//     [HttpPost]
//     [Route("addStudent")]
//     public async Task<ActionResult<Student>> AddStudent(Student request){
//         Students.Add(request);
//         return Ok(Students);
//     }
//     [HttpPut]
//     [Route("updateStudent")]
//     public async Task<ActionResult<Student>> UpdateStudent(Student request){
//         var student = Students.Find(x=>x.Id==request.Id);
//         if(student==null)
//             return BadRequest("No student found!");
//         student.Name=request.Name;
//         student.Age=request.Age;
//         student.city=request.city;

//         return Ok(Students);
//     }
//     [HttpDelete]
//     [Route("deleteStudent")]
//     public async Task<ActionResult<Student>> DeleteStudent(int id){
//         var student = Students.Find(x=>x.Id==id);
//         if(student==null)
//             return BadRequest("No student found!");
//         Students.Remove(student);

//         return Ok(Students);
//     }
// }

public class StudentController : ControllerBase
{
    private readonly ILogger<StudentController> _logger;
    private readonly StudentContext _context;
    public StudentController(ILogger<StudentController> logger, StudentContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    [Authorize]
    public List<Student> GetAllStudents() => _context.getStudents();

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Student>> CreateStudent(string name,int age,string city,string subject)
    {
        var newStudent=new Student{Name=name,Age=age,city=city,subject=subject};
        _context.Students.Add(newStudent);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAllStudents), newStudent);
    }
    [HttpGet]
    [Authorize]
    [Route("getStudent")]
    public async Task<ActionResult<Student>> GetStudent(int id)
    {
        var st = await _context.Students.FindAsync(id);

        if (st == null)
        {
            return NotFound();
        }

        return st;
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> PutTodoItem(int id,string name,int age,string city,string subject)
    {

        var newStudent=new Student{Id=id,Name=name,Age=age,city=city,subject=subject};
        _context.Entry(newStudent).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            //     if (!studentsExists(id))
            // {
            return NotFound();
            // }
            // else
            // {
            //     throw;
            // }
        }

        return Ok();
    }

    [HttpDelete]
    [Authorize]
    
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var st = await _context.Students.FindAsync(id);
        if (st == null)
        {
            return NotFound();
        }

        _context.Students.Remove(st);
        await _context.SaveChangesAsync();

        return Ok();
    }



}
