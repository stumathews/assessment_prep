using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Windows.Markup;
using Microsoft.Win32.SafeHandles;

namespace AssessmentPrep
{
    class Program
    {

        //Normal users can only create new comments, and edit the their own comments.
        public class User
        {
            private string _name;
            private bool isLoggedIn = false;
            private DateTime _lastLoggedIn;
            public User(string name)
            {
                isLoggedIn = false;
                _lastLoggedIn = default(DateTime);
                _name = name;
            }

            public bool IsLoggedIn() => isLoggedIn;

            public DateTime GetLastLoggedInAt() => _lastLoggedIn;

            public void LogIn()
            {
                isLoggedIn = true;
                _lastLoggedIn = DateTime.Now;
            }

            public void LogOut() => isLoggedIn = false;

            public string GetName() => _name;

            public void SetName(string name) => _name = name;

            public virtual bool CanEdit(Comment comment)
            {
                return comment.GetAuthor()._name == _name;
            }

            public virtual bool CanDelete(Comment comment)
            {
                return CanEdit(comment);
            }
        }

        // Moderators have the added ability to delete comments (to remove trolls),
        public class Moderator : User
        {
            public Moderator(String name) : base(name)
            { }

            public override bool CanDelete(Comment comment)
            {
                return true;
            }
        }

        public class Admin : Moderator
        {
            public Admin(String name) : base(name)
            { }

            public override bool CanDelete(Comment comment)
            {
                return true;
            }

            public override bool CanEdit(Comment comment)
            {
                return true;
            }
        }

        public class Comment
        {
            private readonly User _author;
            private string _message;
            private readonly Comment _repliedTo;
            private readonly DateTime _createdAt;

            public Comment(User author, string message, Comment repliedTo = null /*parent*/)
            {
                _author = author;
                _message = message;
                _repliedTo = repliedTo;
                _createdAt = DateTime.Now;
            }

            public string GetMessage() => _message;
            public void SetMessage(String message) => _message = message;

            public DateTime GetCreatedAt() => _createdAt;

            public User GetAuthor() => _author;

            public Comment GetRepliedTo() => _repliedTo;

            public string ToString()
            {
                return _repliedTo == null
                    ? $"«{_message}» by «{_author.GetName()}»"
                    : $"«{_message}» by «{_author.GetName()}» (replied to «{_repliedTo._author.GetName()}»)";
            }

        }

        static void Main(string[] args)
        {
            string markdown = "# This is a first-level (`<h1>`) header #\n"+
                              "## This is a second-level (`<h2>`) header ##\n"+
                              "### And so on. ##";
            var header1 = "# header";
            var header2 = "## header";
            var invalid = "##Invalid";
            var tripple = "###### Header";
            //Approach1(markdown);
            var result1 = MarkdownParser(header1);
            var result2 = MarkdownParser(header2);
            var result3 = MarkdownParser(invalid);
            var trip = MarkdownParser(tripple);
            var helper = new PaginationHelper<char>(new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f' }, 4);
            helper.PageCount();      // should == 2
            helper.ItemCount();      // should == 6
            helper.PageItemCount(0); // should == 4
            helper.PageItemCount(1); // last page - should == 2
            helper.PageItemCount(2); // should == -1 since the page is invalid

            // PageIndex takes an item index and returns the page that it belongs on
            helper.PageIndex(5);     // should == 1 (zero-based index)
            helper.PageIndex(2);     // should == 0
            helper.PageIndex(20);    // should == -1
            helper.PageIndex(-10);   // should == -1
        }

        private static void Approach2(string markdown)
        {
            
        }

        public static string MarkdownParser(string markdown)
        {
            var parts = markdown.ToCharArray();
            int level = 0;
            var isInValid = false;
            string result = string.Empty;
            for (int i = 0; i < parts.Length; i++)
            {

                if (parts[0].Equals(parts[1]) && parts[1].Equals(parts[2]))
                {
                    isInValid = false;
                    break;
                }

                if (parts[i] == '#' && (parts[i+1] == '#' || parts[i + 1] == ' '))
                    level++;
                else
                {
                    break;
                }

            }
            result += level > 0 && !isInValid
                ? $"<h{level}>{new string(markdown.Skip(level).ToArray()).Replace("#", "").TrimStart()}</h{level}>"
                : markdown;
            return result;

        }

        private static void Approach1(string markdown)
        {
            var lines = markdown.Split('\n');
            foreach (var line in lines)
            {
                var parts = line.ToCharArray();
                int level = 0;
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i] == '#')
                        level++;
                    else
                        break;
                }

                var outputLine = new string(line.Skip(level).ToArray());
                outputLine = outputLine.Replace("#", "");
                System.Console.WriteLine(level > 0 
                    ? $"<h{level}>{outputLine}</h{level}>"
                    : line);
            }
        }
    }

    internal class PaginationHelper<T>
    {
        public PaginationHelper(List<char> chars, int i)
        {
            
        }

        public void PageCount()
        {
            

        }

        public void ItemCount()
        {
            

        }

        public void PageItemCount(int p0)
        {
        }

        public void PageIndex(int p0)
        {
        }
    }
}

