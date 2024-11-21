using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using MauiUserApp.Models;

namespace MauiUserApp.Services
{
    public class ApiService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<List<Course>> GetCoursesAsync()
        {
            var courses = await client.GetFromJsonAsync<List<Course>>("http://localhost:3000/courses");
            return courses;
        }

        public async Task<List<Class>> GetClassesAsync(int courseId)
        {
            var classes = await client.GetFromJsonAsync<List<Class>>($"http://localhost:3000/classes?courseid={courseId}");
            return classes;
        }
    }
}