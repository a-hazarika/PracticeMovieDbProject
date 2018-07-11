using System;

namespace PracticeMovieDbProject.ViewModels
{
    public class ErrorViewModel
    {
        public int? ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public ErrorViewModel(int code = 0, string message = null)
        {
            ErrorCode = code;
            ErrorMessage = message;
        }
    }
}