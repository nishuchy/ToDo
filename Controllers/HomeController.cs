using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Models;
using System.Data;
using System.Data.SqlClient;


using Microsoft.Data.SqlClient;
namespace ToDo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            //Student student = new Student();
            //student.Sname = "Nishu";
            //student.Roll = 101;
            //ViewBag.Student = student;

           // ViewBag.Student = GetStudent();
            // Pass the list to the view
            
            
            
            return View(GetStudent());
        }

        public List<Student> GetStudent()
        {
            SqlConnection conn = new SqlConnection(@"Server=localhost\SQLEXPRESS01;Database=University;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;");
            string query = "select * from Student";

            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            List<Student> students = new List<Student>();
            while( reader.Read())
            { 
            Student student = new Student();

             student.Sname=reader["Sname"].ToString();
             student.Roll = Convert.ToInt32(reader["Roll"].ToString());
                student.Address = reader["Address"].ToString();
                student.Email = reader["Email"].ToString();
                student.SID = Convert.ToInt32(reader["SID"].ToString());
                students.Add(student);
            }

            conn.Close();
            return students;
        }


        public ActionResult SavedStudent()
        {
            ViewBag.GetAddress = GetAddress();
            ViewBag.Student = GetStudent();
            return View();
        }



        [HttpPost]

        public ActionResult SavedStudent(Student student)
        {
            string msg = "";

            SqlConnection conn = new SqlConnection(@"Server=localhost\SQLEXPRESS01;Database=University;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;");
            string query = "insert into Student(Sname,Roll, Email, Address)values(@Sname,@Roll, @Email, @Address)";

            SqlCommand cmd = new SqlCommand(query, conn);
          
            cmd.Parameters.AddWithValue("@Sname", student.Sname);
            cmd.Parameters.AddWithValue("@Roll", student.Roll);
            cmd.Parameters.AddWithValue("@Email", student.Email);
            cmd.Parameters.AddWithValue("@Address", student.Address);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            msg = "Saved Success";
            ViewBag.Msg = msg;
            ViewBag.GetAddress = GetAddress();
            ViewBag.Student = GetStudent();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public ActionResult Update()
        {
            Student student = GetStudentByID();

            if (student == null)
            {
                return NotFound(); // Handle case where the student is not found
            }

            return View(student); // Pass the student object to the view
        }
        [HttpPost]
        public ActionResult Update(Student student)
        {
            string msg = "";

            SqlConnection conn = new SqlConnection(@"Server=localhost\SQLEXPRESS01;Database=University;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;");
       
            string query = "update Student set Sname=@sname,Roll=@roll, Email=@email, Address=@address where SID=@SID";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Sname", student.Sname);
            cmd.Parameters.AddWithValue("@Roll", student.Roll);
            cmd.Parameters.AddWithValue("@Email", student.Email);
            cmd.Parameters.AddWithValue("@Address", student.Address);
            cmd.Parameters.AddWithValue("@SID", student.SID);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            msg = "Update Success";
            ViewBag.Msg = msg;
           
            return View();
        }

        public Student GetStudentByID()
        {
            SqlConnection conn = new SqlConnection(@"Server=localhost\SQLEXPRESS01;Database=University;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;");
            string query = "select * from Student where SID=@SID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@SID", Convert.ToInt32(HttpContext.Request.Query["sid"]));
            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            Student student = null; // Initialize to null to handle no results

            if (reader.Read())
            {
                student = new Student
                {
                    Sname = reader["Sname"].ToString(),
                    Roll = Convert.ToInt32(reader["Roll"].ToString()),
                    Address = reader["Address"].ToString(),
                    Email = reader["Email"].ToString(),
                    SID = Convert.ToInt32(reader["SID"].ToString())
                };
            }

            conn.Close();
            return student;
        }
        public List<String> GetAddress()
        {
            return new List<string> { "Chittagong", "Dhaka", "Khulna"};
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
           

            SqlConnection conn = new SqlConnection(@"Server=localhost\SQLEXPRESS01;Database=University;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;");

            string query = "delete from Student where SID=@SID";

            SqlCommand cmd = new SqlCommand(query, conn);

    
            cmd.Parameters.AddWithValue("@SID", id);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();

            return RedirectToAction("GetAll");
        }

    }


}
