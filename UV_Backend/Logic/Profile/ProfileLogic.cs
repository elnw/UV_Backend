using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UV_Backend.Data;
using UV_Backend.Entities;
using UV_Backend.Entities.Responses.Profile;
using UV_Backend.Helpers.Logic;
using UV_Backend.Models;

namespace UV_Backend.Logic.Profile
{
    public class ProfileLogic
    {
        private readonly ProfileData _profileData;
        private readonly UserManager<IdentityUser> _userManager;
        public ProfileLogic(ProfileData profileData, UserManager<IdentityUser> userManager)
        {
            _profileData = profileData;
            _userManager = userManager;
        }
        public async Task<List<QuestionaryResponse>> RetrieveQuestionary()
        {
            var rawQuestionary = await _profileData.FetchQuestionary();

            var groupQuestions = rawQuestionary.GroupBy(q => q.Question.Id);

            var response = new List<QuestionaryResponse>();

            foreach (var questionary in groupQuestions)
            {
                QuestionaryResponse currentQuestion = new QuestionaryResponse();

                foreach (var item in questionary)
                {
                    currentQuestion.Question = item.Question;
                    if(currentQuestion.PossibleAnswers == null)
                    {
                        currentQuestion.PossibleAnswers = new List<Models.Answer>();
                    }
                    currentQuestion.PossibleAnswers.Add(item.Answer);
                }
                response.Add(currentQuestion);
            }

            return response;
        }

        public async Task<ApiResponse<List<Models.Profile>>> RetrieveProfilesByUser(string userId)
        {
            if (String.IsNullOrWhiteSpace(userId)) return new ApiResponse<List<Models.Profile>> { message = "El id provisto es inválido" };

            return new ApiResponse<List<Models.Profile>> { data = await _profileData.FetchProfileByUser(userId) };

        }

        public async Task<ApiResponse<bool>> DeleteProfile(int profileId)
        {
            if (profileId <0) return new ApiResponse<bool> { message = "El id provisto es inválido", data = false };
            var response = await _profileData.DeleteProfile(profileId);

            return new ApiResponse<bool> { message = response.Item2, data = response.Item1 };
        }

        public async Task<ApiResponse<bool>> UpdateProfile(Models.Profile profile)
        {
            if (profile == null) return new ApiResponse<bool> { message = "No se recibió información" };
            if(profile.Id == 0) return new ApiResponse<bool> { message = "No se especificó el perfil a modificar" };

            profile.ScoreDescription = profile.Score == null? null : LogicHelpers.GetDescriptionForScore(Convert.ToInt32(profile.Score));
            var response = await _profileData.UpdateProfile(profile);

            return new ApiResponse<bool> { message = response.Item2, data =response.Item1 };
        }

        public async Task<ApiResponse<bool>> CreateProfile(Entities.Requests.Profile.CreateProfileRequest createProfileRequest)
        {
            if(createProfileRequest == null) return new ApiResponse<bool> { message = "No se recibió información" };

            if(createProfileRequest.UserId == null) return new ApiResponse<bool> { message = "No se recibió información del usuario" };

            var selectedUser = await _userManager.FindByIdAsync(createProfileRequest.UserId); 
            if(selectedUser == null)
            {
                return new ApiResponse<bool> { message = "No se encontró el usuario especificado", data = false };
            }
            if(createProfileRequest.Profile.Birthday == DateTime.MinValue || 
                createProfileRequest.Profile.Score == null || 
                createProfileRequest.Profile.Name == null)
            {
                return new ApiResponse<bool> { message = "No se recibió información completa del perfil", data = false };
            }

            createProfileRequest.Profile.User = selectedUser;
            createProfileRequest.Profile.ScoreDescription = LogicHelpers.GetDescriptionForScore(Convert.ToInt32(createProfileRequest.Profile.Score));

            var result = await _profileData.CreateProfile(createProfileRequest.Profile);
            return new ApiResponse<bool> { message = result.Item2, data = result.Item1 };   

        }

        public async Task<string> RetrieveScoreDescriptionByProfile(int profile)
        {
            return await _profileData.RetriveScoreFromProfile(profile);
        }

    }
}
