using MauiUserApp.Models;
using MauiUserApp.Services;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MauiUserApp
{
    public partial class MainPage : ContentPage
    {
        private ApiService _apiService;
        private bool _isRefreshing = false;

        public MainPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            LoadData();
        }

        // Fetch and load data (for initial load and refresh)
        private async void LoadData()
        {
            if (_isRefreshing) return;  // Prevent triggering refresh when already refreshing

            _isRefreshing = true;  // Set refreshing to true when loading data
            CoursesRefreshView.IsRefreshing = true; // Enable the refresh indicator

            // Fetch courses
            var courses = await _apiService.GetCoursesAsync();
            // Fetch all classes and group them by CourseId
            var allClasses = new List<Class>();

            foreach (var course in courses)
            {
                var classes = await _apiService.GetClassesAsync(course.CourseId);
                allClasses.AddRange(classes);
            }

            // Group the classes by CourseId and ensure there are no duplicates by removing identical ClassId entries
            var groupedClasses = allClasses
                .GroupBy(c => c.CourseId)
                .ToDictionary(g => g.Key, g => g.DistinctBy(c => c.ClassId).ToList());

            // Assign to the CollectionView with unique courses and their associated classes
            CoursesCollectionView.ItemsSource = courses.Select(course => new CourseWithClasses
            {
                Course = course,
                Classes = groupedClasses.ContainsKey(course.CourseId) ? groupedClasses[course.CourseId] : new List<Class>()
            }).ToList();

            // Set refreshing to false after data is loaded
            _isRefreshing = false;
            CoursesRefreshView.IsRefreshing = false;
        }

        // Handle the Refresh event when the user pulls to refresh
        private async void OnRefresh(object sender, System.EventArgs e)
        {
            if (_isRefreshing) return;  // Prevent triggering refresh if it's already refreshing

            await Task.Delay(500);  // Optional delay for refresh effect
            LoadData();  // Reload the data
        }

        private void CoursesCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is CourseWithClasses selectedCourseWithClasses)
            {
                // Handle the selection logic, for example, display more details
            }
        }
    }

    // Helper class to store Course with associated Classes
    public class CourseWithClasses
    {
        public Course Course { get; set; }
        public List<Class> Classes { get; set; }
    }
}