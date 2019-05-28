using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private string path = Path.GetFullPath("NewFolder");

        private static string[] SentenceTerminatingChars = new[]
        {
            ".", "!", "?"
        };

        [HttpPost("[action]")]
        public async Task<IActionResult> FileUpload([FromForm]IFormCollection vals)
        {
            var clearResult = ClearFolder(); //Clear any files out of the temp folder
            try
            {
                if (clearResult)
                {
                    var files = vals.Files;//Get the uploaded file

                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = file.FileName;

                            var serverFileStream = System.IO.File.CreateText(path + "\\" + fileName);
                            await file.CopyToAsync(serverFileStream.BaseStream);//Copy the file to the temp folder
                            serverFileStream.Close();
                        }
                    }
                }

                else
                {
                    ModelState.AddModelError("", "Cannot clear temp folder");
                    return BadRequest(ModelState);
                }
            }
            catch (Exception x)
            {
                return Unauthorized();
            }
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<string> ParseFileSentences()
        {
            var splitBy = new List<string>();
            splitBy.AddRange(new string[] { "\\n" });
            splitBy.AddRange(SentenceTerminatingChars);//split by { newline, '.', '!', '?' }
            var file = Directory.GetFiles(path).First();
            var lines = System.IO.File.ReadAllLines(file);
            var textLines = lines.AsQueryable().Where(x => x.ToCharArray().Any(c => Char.IsLetter(c)));//get all lines of text that contain actual text (ie. eliminate '-------------')
            var linesAsString = string.Join("", textLines);

            var unsortedLines = linesAsString.Split(splitBy.ToArray(), StringSplitOptions.RemoveEmptyEntries).AsQueryable();
            List<string> unsortedSentences = new List<string>();


            foreach (var line in unsortedLines)
            {
                var newLine = line.Trim();
                if (newLine.StartsWith("\""))
                {
                    newLine = newLine.Replace("\"", "");//remove leading "
                }

                unsortedSentences.Add(newLine.Trim());
            }

            var sortedSentences = unsortedSentences.OrderBy(x => x).Select(x => x + "\n").AsEnumerable();
            return string.Join("", sortedSentences);//return sorted sentences
        }

        private bool ClearFolder()
        {
           var files= Directory.GetFiles(path);
            if (files.Length > 0)
            {
                foreach(var file in files)
                {
                    System.IO.File.Delete(file);
                }
            }

            return Directory.GetFiles(path).Length == 0;
        }
    }
}
