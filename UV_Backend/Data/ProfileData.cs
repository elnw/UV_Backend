using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UV_Backend.Entities.Responses.Profile;
using UV_Backend.Helpers.Logic;
using UV_Backend.Models;

namespace UV_Backend.Data
{
    public class ProfileData
    {
        private readonly AppDbContext _appDbContext;
        public ProfileData(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async  Task<List<QuestionAnswer>> FetchQuestionary()
        {

            return await _appDbContext.questionAnswers.Include("Question").Include("Answer").ToListAsync();
            
        }

        public async Task<List<Profile>> FetchProfileByUser(string userId)
        {
            return await _appDbContext.Profile.Where(p => p.User.Id == userId).ToListAsync();
        }

        public async Task<(bool, string)> DeleteProfile(int profileId)
        {
            try
            {
                var Profile = await _appDbContext.Profile.FindAsync(profileId);
                if (Profile != null)
                {
                    _appDbContext.Profile.Remove(Profile);
                    await _appDbContext.SaveChangesAsync();
                }
                return (true, "");
            }
            catch(Exception ex)
            {
                return (false, ex.Message);
            }
            
        }

        public async Task<(bool, string)> UpdateProfile(Profile newProfile)
        {
            try
            {
                var modifiedProfile = _appDbContext.Profile.Attach(newProfile);
                modifiedProfile.State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                modifiedProfile.Property(x=> x.Name).IsModified = newProfile.Name != null;
                modifiedProfile.Property("Birthday").IsModified = newProfile.Birthday != DateTime.MinValue;
                modifiedProfile.Property("Score").IsModified = newProfile.Score != null;
                modifiedProfile.Property("ScoreDescription").IsModified = newProfile.Score != null;


                await _appDbContext.SaveChangesAsync();
                return (true, "");
            }catch(Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool, string)> CreateProfile(Profile newProfile)
        {
            try
            {
                _appDbContext.Profile.Add(newProfile);
                await _appDbContext.SaveChangesAsync();
                return (true, "");
            }catch(Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<string> RetriveScoreFromProfile(int profileId)
        {
            var profile =  await _appDbContext.Profile.FindAsync(profileId);

            return profile.ScoreDescription;

        }

    }
}
