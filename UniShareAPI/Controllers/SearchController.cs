using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;
using UniShareAPI.Models.DTO.Requests.Filter;
using UniShareAPI.Models.DTO.Requests.Search.Comments;
using UniShareAPI.Models.DTO.Requests.Search.Course;
using UniShareAPI.Models.DTO.Requests.Search.People;
using UniShareAPI.Models.DTO.Requests.Search.Ratings;
using UniShareAPI.Models.DTO.Requests.Search.Review;
using UniShareAPI.Models.DTO.Requests.Search.UserReview;
using UniShareAPI.Models.DTO.Response.Search.Comments;
using UniShareAPI.Models.DTO.Response.Search.Courses;
using UniShareAPI.Models.DTO.Response.Search.People;
using UniShareAPI.Models.DTO.Response.Search.ProfileReviews;
using UniShareAPI.Models.DTO.Response.Search.Ratings;
using UniShareAPI.Models.DTO.Response.Search.Reviews;
using UniShareAPI.Models.Extensions;
using UniShareAPI.Models.Relations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UniShareAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public SearchController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost]
        [Route("user/reviews")]
        public async Task<IActionResult> SearchUserReviewsAsync([FromBody] UserReviewFilter filter)
        {
            UserReviewFilterResultResponse results = await SearchUserReviewsWithTextAsync(filter);
            return Ok(results);
        }

        private async Task<UserReviewFilterResultResponse> SearchUserReviewsWithTextAsync(UserReviewFilter filter)
        {
            string search = filter.Search;
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName.Equals(filter.Username));
            string profileId = user.Id;

            int count = 0;
            IQueryable<ProfileReviewResponse> reviews;

            if (search == null)
            {
                count = _appDbContext.Courses
               .Join(_appDbContext.Reviews, u => u.Id, uir => uir.CourseId, (u, uir) => new { u, uir })
               .Where(m => m.uir.UserId.Equals(profileId)).Count();

                reviews = _appDbContext.Courses
                .Join(_appDbContext.Reviews, u => u.Id, uir => uir.CourseId, (u, uir) => new { u, uir })
                .Where(m => m.uir.UserId.Equals(profileId))
                .Select(p => new ProfileReviewResponse
                {
                    Text = p.uir.Text,
                    Added = p.uir.AddedDate,
                    Updated = p.uir.UpdatedDate,
                    CourseId = p.u.Id,
                    Difficulty = p.uir.Difficulty,
                    Environment = p.uir.Environment,
                    Fulfilling = p.uir.Fulfilling,
                    Grading = p.uir.Grading,
                    Litterature = p.uir.Litterature,
                    Name = p.u.Name,
                    Overall = p.uir.Overall,
                    University = p.u.University
                });
            }
            else
            {
                count = _appDbContext.Courses
                   .Join(_appDbContext.Reviews, u => u.Id, uir => uir.CourseId, (u, uir) => new { u, uir })
                   .Where(m => m.uir.UserId.Equals(profileId) &&
                            (m.uir.Text.Contains(search) ||
                            m.uir.AddedDate.ToString().Contains(search) ||
                            m.u.Credits.Equals(search) ||
                            m.u.Code.Contains(search) ||
                            m.u.Name.Contains(search) ||
                            m.u.University.Contains(search)
                            )).Count();

                reviews = _appDbContext.Courses
               .Join(_appDbContext.Reviews, u => u.Id, uir => uir.CourseId, (u, uir) => new { u, uir })
                .Where(m => m.uir.UserId.Equals(profileId) &&
                            (m.uir.Text.Contains(search) ||
                            m.uir.AddedDate.ToString().Contains(search) ||
                            m.u.Credits.Equals(search) ||
                            m.u.Code.Contains(search) ||
                            m.u.Name.Contains(search) ||
                            m.u.University.Contains(search)
                            ))
                .Select(p => new ProfileReviewResponse
                {
                    Text = p.uir.Text,
                    Added = p.uir.AddedDate,
                    Updated = p.uir.UpdatedDate,
                    CourseId = p.u.Id,
                    Difficulty = p.uir.Difficulty,
                    Environment = p.uir.Environment,
                    Fulfilling = p.uir.Fulfilling,
                    Grading = p.uir.Grading,
                    Litterature = p.uir.Litterature,
                    Name = p.u.Name,
                    Overall = p.uir.Overall,
                    University = p.u.University,
                    Code = p.u.Code
                });
            }
            
            Pagination pagination = CalculateOffsets(count, filter.Page, filter.ResultsPerPage);
            List<ProfileReviewResponse> filteredResults = ApplyOrderingUserReviewFilters(reviews, filter).Skip(pagination.PageFirstResultIndex).Take(pagination.ResultsPerPage).ToList();

            var peopleResult = new UserReviewFilterResultResponse
            {
                Pagination = pagination,
                ProfileReviews = filteredResults,
                TotalMatches = count
            };
            
            return peopleResult;
        }

        [HttpPost]
        [Route("user/ratings")]
        public async Task<IActionResult> SearchRatingsAsync([FromBody] RatingsFilter filter)
        {
            RatingsFilterResultResponse results = await SearchUserRatingsWithTextAsync(filter);
            return Ok(results);
        }

        private async Task<RatingsFilterResultResponse> SearchUserRatingsWithTextAsync(RatingsFilter filter)
        {
            string search = filter.Search;
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName.Equals(filter.ProfileId));
            string profileId = user.Id;
            int count = 0;
            IQueryable<RatingsResponse> reviews;

            if (search == null)
            {
                count = _appDbContext.Courses
               .Join(_appDbContext.UserCourses, u => u.Id, uir => uir.CourseId, (u, uir) => new { u, uir })
               .Where(m => m.uir.UserId.Equals(profileId)).Count();

                reviews = _appDbContext.Courses
               .Join(_appDbContext.UserCourses, u => u.Id, uir => uir.CourseId, (u, uir) => new { u, uir })
                .Where(m => m.uir.UserId.Equals(profileId))
                .Select(p => new RatingsResponse
                {
                    Name = p.u.Name,
                    Code = p.u.Code,
                    Credits = p.u.Credits,
                    Rating = p.uir.Rating,
                    University = p.u.University,
                    City = p.u.City,
                    CourseId = p.u.Id
                });
            }
            else
            {
                count = _appDbContext.Courses
               .Join(_appDbContext.UserCourses, u => u.Id, uir => uir.CourseId, (u, uir) => new { u, uir })
               .Where(m => m.uir.UserId.Equals(profileId) &&
                (m.u.Name.Contains(search) ||
                m.u.University.Contains(search) ||
                m.u.Credits.Equals(search) ||
                m.u.Code.Contains(search) ||
                m.u.Country.Contains(search) ||
                m.u.City.Contains(search)
                )).Count();

                reviews = _appDbContext.Courses
               .Join(_appDbContext.UserCourses, u => u.Id, uir => uir.CourseId, (u, uir) => new { u, uir })
                .Where(m => m.uir.UserId.Equals(profileId) &&
                (m.u.Name.Contains(search) ||
                m.u.University.Contains(search) ||
                m.u.Credits.Equals(search) ||
                m.u.Code.Contains(search) ||
                m.u.Country.Contains(search) ||
                m.u.City.Contains(search)
                ))
                .Select(p => new RatingsResponse
                {
                    Name = p.u.Name,
                    Code = p.u.Code,
                    Credits = p.u.Credits,
                    Rating = p.uir.Rating,
                    University = p.u.University,
                    City = p.u.City,
                    CourseId = p.u.Id
                });
            }

            Pagination pagination = CalculateOffsets(count, filter.Page, filter.ResultsPerPage);
            List<RatingsResponse> filteredResults = ApplyOrderingRatingsFilters(reviews, filter).Skip(pagination.PageFirstResultIndex).Take(pagination.ResultsPerPage).ToList();

            var peopleResult = new RatingsFilterResultResponse
            {
                Pagination = pagination,
                Ratings = filteredResults,
                TotalMatches = count
            };

            return peopleResult;
        }

        [HttpPost]
        [Route("reviews")]
        public IActionResult SearchReviews([FromBody] ReviewFilter filter)
        {
            ReviewFilterResultResponse results = SearchReviewsWithText(filter);
            return Ok(results);
        }
        private ReviewFilterResultResponse SearchReviewsWithText(ReviewFilter filter)
        {
            string search = filter.Search;
            int courseId = filter.CourseId;
            int count = 0;
            IQueryable<ReviewResponse> reviews;

            if (search == null)
            {
                count = _appDbContext.User
                .Join(_appDbContext.Reviews, u => u.Id, uir => uir.UserId, (u, uir) => new { u, uir })
                .Where(m => m.uir.CourseId.Equals(courseId)).Count();

                reviews = _appDbContext.User
                .Join(_appDbContext.Reviews, u => u.Id, uir => uir.UserId, (u, uir) => new { u, uir })
                .Where(m => m.uir.CourseId.Equals(courseId))
                .Select(p => new ReviewResponse
                {
                    CourseId = p.uir.CourseId,
                    Difficulty = p.uir.Difficulty,
                    Environment = p.uir.Environment,
                    Fulfilling = p.uir.Fulfilling,
                    Grading = p.uir.Grading,
                    Litterature = p.uir.Litterature,
                    Overall = p.uir.Overall,
                    Username = p.u.UserName,
                    Image = p.u.Image,
                    Text = p.uir.Text,
                    Added = p.uir.AddedDate,
                    Updated = p.uir.UpdatedDate
                });
            }
            else
            {
                count = _appDbContext.User
                        .Join(_appDbContext.Reviews, u => u.Id, uir => uir.UserId, (u, uir) => new { u, uir })
                        .Where(m => m.uir.CourseId.Equals(courseId) &&
                        (m.u.UserName.ToLower().Contains(search) ||
                        m.uir.Text.ToLower().Contains(search) ||
                        m.uir.AddedDate.ToString().Contains(search) ||
                        m.uir.UpdatedDate.ToString().Contains(search))).Count();

                reviews = _appDbContext.User
                .Join(_appDbContext.Reviews, u => u.Id, uir => uir.UserId, (u, uir) => new { u, uir })
                .Where(m => m.uir.CourseId.Equals(courseId) &&
                        (m.u.UserName.ToLower().Contains(search) ||
                        m.uir.Text.ToLower().Contains(search) ||
                        m.uir.AddedDate.ToString().Contains(search) ||
                        m.uir.UpdatedDate.ToString().Contains(search)))
                .Select(p => new ReviewResponse
                {
                    CourseId = p.uir.CourseId,
                    Difficulty = p.uir.Difficulty,
                    Environment = p.uir.Environment,
                    Fulfilling = p.uir.Fulfilling,
                    Grading = p.uir.Grading,
                    Litterature = p.uir.Litterature,
                    Overall = p.uir.Overall,
                    Username = p.u.UserName,
                    Image = p.u.Image,
                    Text = p.uir.Text,
                    Added = p.uir.AddedDate,
                    Updated = p.uir.UpdatedDate
                });
            }

            Pagination pagination = CalculateOffsets(count, filter.Page, filter.ResultsPerPage);
            List<ReviewResponse> filteredResults = ApplyOrderingReviewFilters(reviews, filter).Skip(pagination.PageFirstResultIndex).Take(pagination.ResultsPerPage).ToList();

            var peopleResult = new ReviewFilterResultResponse
            {
                Pagination = pagination,
                Reviews = filteredResults,
                TotalMatches = count
            };

            return peopleResult;
        }

        [HttpPost]
        [Route("comments")]
        public async Task<IActionResult> SearchComments([FromBody] CommentsFilter filter)
        {
            CommentFilterResultResponse results = await SearchCommentsWithTextAsync(filter);
            return Ok(results);
        }

        private async Task<CommentFilterResultResponse> SearchCommentsWithTextAsync(CommentsFilter filter)
        {
            string search = filter.Search;
            var user = await _appDbContext.User.FirstOrDefaultAsync(x => x.UserName == filter.ProfileId);

            string profileId = user.Id;
            int count = 0;
            IQueryable<CommentResponse> comments;

            if (search == null)
            {
                count = _appDbContext.User
               .Join(_appDbContext.Comments, u => u.Id, uir => uir.ProfileId, (u, uir) => new { u, uir })
               .Where(m => m.uir.ProfileId.Equals(profileId)).Count();

                comments = _appDbContext.User
                    .Join(_appDbContext.Comments, u => u.Id, uir => uir.ProfileId, (u, uir) => new { u, uir })
                    .Where(m => m.uir.ProfileId.Equals(profileId))
                .Select(p => new CommentResponse
                {
                    CommentId = p.uir.Id,
                    Text = p.uir.Text,
                    Username = p.u.UserName,
                    Date = p.uir.Date,
                    AuthorId = p.uir.AuthorId,
                    ProfileId = p.uir.ProfileId
                });
            }
            else
            {
                count = _appDbContext.User
                .Join(_appDbContext.Comments, u => u.Id, uir => uir.ProfileId, (u, uir) => new { u, uir })
                .Where(m => m.uir.ProfileId.Equals(profileId) &&
                (m.u.UserName.ToLower().Contains(search) ||
                m.uir.Text.ToLower().Contains(search))).Count();

                comments = _appDbContext.User
                    .Join(_appDbContext.Comments, u => u.Id, uir => uir.ProfileId, (u, uir) => new { u, uir })
                    .Where(m => m.uir.ProfileId.Equals(profileId) &&
                    (m.u.UserName.ToLower().Contains(search) ||
                    m.uir.Text.ToLower().Contains(search)))
                .Select(p => new CommentResponse
                {
                    CommentId = p.uir.Id,
                    Text = p.uir.Text,
                    Username = p.u.UserName,
                    Date = p.uir.Date,
                    AuthorId = p.uir.AuthorId,
                    ProfileId = p.uir.ProfileId
                });
            }

            Pagination pagination = CalculateOffsets(count, filter.Page, filter.ResultsPerPage);
            List<CommentResponse> filteredResults = ApplyOrderingCommentsFilters(comments, filter).Skip(pagination.PageFirstResultIndex).Take(pagination.ResultsPerPage).ToList();

            //Add images to the authors of the search result.
            foreach(CommentResponse commentResponse in filteredResults)
            {
                var Author = await _appDbContext.User.FirstOrDefaultAsync(x => x.Id == commentResponse.AuthorId);
                commentResponse.Image = Author.Image;
            }

            var commentResult = new CommentFilterResultResponse
            {
                Pagination = pagination,
                Comments = filteredResults,
                TotalMatches = count
            };

            return commentResult;
        }

        [HttpPost]
        [Route("courses")]
        public async Task<IActionResult> SearchCoursesAsync([FromBody] CourseFilter filter)
        {
            CourseFilterResultResponse results = await SearchCoursesWithTextAsync(filter);
            return Ok(results);
        }

        private async Task<CourseFilterResultResponse> SearchCoursesWithTextAsync(CourseFilter filter)
        {
            string search = filter.Search;

            var count = _appDbContext.Courses.Where(
                x => x.Name.ToLower().Contains(search) ||
                x.Code.ToLower().Contains(search) ||
                x.City.ToLower().Contains(search) ||
                x.Credits.ToString().Contains(search) ||
                x.Added.ToString().Contains(search) ||
                x.Country.ToLower().Contains(search) ||
                x.University.ToLower().Contains(search) ||
                x.Link.ToLower().Contains(search)).Count();

            var courses = _appDbContext.Courses.Where(
                x => x.Name.ToLower().Contains(search) ||
                x.Code.ToLower().Contains(search) ||
                x.City.ToLower().Contains(search) ||
                x.Credits.ToString().Contains(search) ||
                x.Added.ToString().Contains(search) ||
                x.Country.ToLower().Contains(search) ||
                x.University.ToLower().Contains(search) ||
                x.Link.ToLower().Contains(search) 
                )
                .Select(p => new CourseResponse
                {
                    Name = p.Name,
                    University = p.University,
                    Added = p.Added,
                    City = p.City,
                    Code = p.Code,
                    Country = p.Country,
                    Credits = p.Credits,
                    Link = p.Link,
                    Id = p.Id,
                    Rating = 0,
                    InActiveDegree = false
                }).ToList();

            //Find rating for all courses. Might have to rework later for performance...
            foreach (CourseResponse courseResponse in courses)
            {
                var course = await _appDbContext.UserCourses.FirstOrDefaultAsync(x => x.CourseId.Equals(courseResponse.Id));

                if (course != null)
                {
                    courseResponse.Rating = _appDbContext.UserCourses.Where(r => r.CourseId == courseResponse.Id).Average(r => r.Rating);

                }
            }

            Pagination pagination = CalculateOffsets(count, filter.Page, filter.ResultsPerPage);
            List<CourseResponse> filteredResults = ApplyOrderingCoursesFilters(courses.AsQueryable(), filter).Skip(pagination.PageFirstResultIndex).Take(pagination.ResultsPerPage).ToList();

            //Check if the sorted courses is in the active degree.
            var user = await _appDbContext.Users.FirstOrDefaultAsync(x => x.Id == filter.ActiveDegreeUserId);
            if(user != null && user.ActiveDegreeId != null)
            {
                foreach (CourseResponse courseResponse in filteredResults)
                {
                    int courseId = courseResponse.Id;

                    //Use the DegreeId and CourseId to get the composite key row.
                    var courseIsInDegree = _appDbContext.DegreeCourses.Any(x => x.DegreeId.Equals(user.ActiveDegreeId) && x.CourseId == courseId);
                    //If we have a course in the active degree, set it as active!
                    if (courseIsInDegree)
                    {
                        courseResponse.InActiveDegree = true;
                    }
                }
            }

            var coursesResponse = new CourseFilterResultResponse
            {
                Pagination = pagination,
                Courses = filteredResults,
                TotalMatches = count
            };

            return coursesResponse;
        }

        [HttpPost]
        [Route("users")]
        public IActionResult SearchUsers([FromBody] UserFilter filter)
        {
            PeopleFilterResultResponse results = SearchUsersWithText(filter);
            return Ok(results);
        }

        private PeopleFilterResultResponse SearchUsersWithText(UserFilter filter)
        {
            string search = filter.Search;

            var count = _appDbContext.User.Where(
                x => x.Firstname.ToLower().Contains(search) ||
                x.Lastname.ToLower().Contains(search) ||
                x.UserName.ToLower().Contains(search) ||
                x.Joined.ToString().Contains(search) ||
                x.LastSeenDate.ToString().Contains(search) ||
                x.Visits.ToString().Contains(search)).Count();

            var users = _appDbContext.User.Where(
                x => x.Firstname.ToLower().Contains(search) ||
                x.Lastname.ToLower().Contains(search) ||
                x.UserName.ToLower().Contains(search) ||
                x.Joined.ToString().Contains(search) ||
                x.LastSeenDate.ToString().Contains(search) ||
                x.Visits.ToString().Contains(search))
                .Select(p => new UserResponse
                {
                    Firstname = p.Firstname,
                    Lastname = p.Lastname,
                    Joined = p.Joined,
                    LastSeenDate = p.LastSeenDate,
                    Username = p.UserName,
                    Visits = p.Visits,
                    Image = p.Image,
                    Id = p.Id,
                });

            Pagination pagination = CalculateOffsets(count, filter.Page, filter.ResultsPerPage);
            List<UserResponse> filteredResults = ApplyOrderingUsersFilters(users, filter).Skip(pagination.PageFirstResultIndex).Take(pagination.ResultsPerPage).ToList();

            var peopleResult = new PeopleFilterResultResponse
            {
                Pagination = pagination,
                Users = filteredResults,
                TotalMatches = count
            };

            return peopleResult;
        }

        //TODO, Refactor to generics later...
        private static List<RatingsResponse> ApplyOrderingRatingsFilters(IQueryable<RatingsResponse> users, RatingsFilter filter)
        {
            return filter.Order switch
            {
                "Ascending" => users.OrderBy(filter.Option, true).ToList(),
                "Descending" => users.OrderBy(filter.Option, false).ToList(),
                _ => new List<RatingsResponse>(),
            };
        }
        private static List<ProfileReviewResponse> ApplyOrderingUserReviewFilters(IQueryable<ProfileReviewResponse> users, UserReviewFilter filter)
        {
            return filter.Order switch
            {
                "Ascending" => users.OrderBy(filter.Option, true).ToList(),
                "Descending" => users.OrderBy(filter.Option, false).ToList(),
                _ => new List<ProfileReviewResponse>(),
            };
        }
        private static List<ReviewResponse> ApplyOrderingReviewFilters(IQueryable<ReviewResponse> users, ReviewFilter filter)
        {
            return filter.Order switch
            {
                "Ascending" => users.OrderBy(filter.Option, true).ToList(),
                "Descending" => users.OrderBy(filter.Option, false).ToList(),
                _ => new List<ReviewResponse>(),
            };
        }
        private static List<CommentResponse> ApplyOrderingCommentsFilters(IQueryable<CommentResponse> users, CommentsFilter filter)
        {
            return filter.Order switch
            {
                "Ascending" => users.OrderBy(filter.Option, true).ToList(),
                "Descending" => users.OrderBy(filter.Option, false).ToList(),
                _ => new List<CommentResponse>(),
            };
        }
        private static List<UserResponse> ApplyOrderingUsersFilters(IQueryable<UserResponse> users, UserFilter filter)
        {
            return filter.Order switch
            {
                "Ascending" => users.OrderBy(filter.Option, true).ToList(),
                "Descending" => users.OrderBy(filter.Option, false).ToList(),
                _ => new List<UserResponse>(),
            };
        }

        private static List<CourseResponse> ApplyOrderingCoursesFilters(IQueryable<CourseResponse> courses, CourseFilter filter)
        {
            return filter.Order switch
            {
                "Ascending" => courses.OrderBy(filter.Option, true).ToList(),
                "Descending" => courses.OrderBy(filter.Option, false).ToList(),
                _ => new List<CourseResponse>(),
            };
        }

        private static Pagination CalculateOffsets(int count, int page, int resultPerPage)
        {
            int TotalPages = (count + resultPerPage - 1) / resultPerPage;
            int PageFirstResultIndex = (page - 1) * resultPerPage;

            return new Pagination
            {
                ResultsPerPage = resultPerPage,
                TotalPages = TotalPages,
                PageFirstResultIndex = PageFirstResultIndex
            };
        }
    }
}
