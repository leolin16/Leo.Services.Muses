using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Leo.Services.Muses.Entities;
using Leo.Services.Muses.Interfaces;
using Leo.Services.Muses.Models;
using Leo.Services.Muses.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Leo.Services.Muses.Controllers
{
    [Route("api/[controller]")]
    public class SingersController : Controller
    {
		private IMusesRepository _musesRepository;
		private ILogger<SingersController> _logger;

        public SingersController(IMusesRepository musesRepository, ILogger<SingersController> logger)
        {
			_musesRepository = musesRepository;
            _logger = logger;
        }

        public static bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }
        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> GetSingers()
        {
			//return Ok(SingersDataStore.Current.Singers);
			IEnumerable<Singer> singerEntities = await _musesRepository.GetSingersAsync();
			var results = Mapper.Map<IEnumerable<SingerWithoutSongsDto>>(singerEntities);
			return Ok(results);

        }
        
        // GET api/values/5
        [HttpGet("{singerIdOrName}")]
        public IActionResult GetSinger(string singerIdOrName, bool includeSongs = false)
        {
			//SingerDto singerToReturn;
			Singer singerEntity;
            if(IsInt(singerIdOrName)) {
                //singerToReturn = SingersDataStore.Current.Singers.FirstOrDefault(singer => singer.Id == Convert.ToInt32(singerIdOrName));
				singerEntity = _musesRepository.GetSingerAsync(Convert.ToInt32(singerIdOrName), includeSongs).Result;
            } else {
                //singerToReturn = SingersDataStore.Current.Singers.FirstOrDefault(singer => singer.Name.ToLower().Replace(" ", "") == singerIdOrName.Replace("-", " ").ToLower().Replace(" ", ""));
				singerEntity = _musesRepository.GetSingerAsync(singerIdOrName, includeSongs).Result;
            }
            //if(singerToReturn == null) {
            //    _logger.LogInformation($"Singer with id or name: {singerIdOrName} wasn't found");
            //    return NotFound();
            //} else {
            //    return Ok(singerToReturn);
            //}
			if(singerEntity == null) {
				return NotFound();
			}
			if(includeSongs) {
				var singerResult = Mapper.Map<SingerDto>(singerEntity);
				return Ok(singerResult);
			}
			var singerWithoutSongsResult = Mapper.Map<SingerWithoutSongsDto>(singerEntity);
			return Ok(singerWithoutSongsResult);
        }

        [HttpGet("{singerIdOrName}/criticisms")]
        public IActionResult GetCriticismsOfSinger(string singerIdOrName)
        {
            Singer singerEntity;
            if (IsInt(singerIdOrName))
            {
                singerEntity = _musesRepository.GetSingerAsync(Convert.ToInt32(singerIdOrName), true).Result;
            }
            else
            {
                singerEntity = _musesRepository.GetSingerAsync(singerIdOrName, true).Result;
            }

            if (singerEntity == null)
            {
                return NotFound();
            }
			var criticismsResult = Mapper.Map<IEnumerable<CriticismWithoutSingerDto>>(singerEntity.Criticisms);
			return Ok(criticismsResult);
        }

        [HttpGet("{singerIdOrName}/criticisms/{criticismId}", Name = "GetCriticismOfSinger")]
        public IActionResult GetCriticismOfSinger(string singerIdOrName, string criticismId)
        {
			//SingerDto singerToReturn;
			//SongDto songToReturn;
			Singer singerEntity;
			Criticism criticismEntity;
            if (IsInt(singerIdOrName))
            {
                //singerToReturn = SingersDataStore.Current.Singers.FirstOrDefault(singer => singer.Id == Convert.ToInt32(singerIdOrName));
				singerEntity = _musesRepository.GetSingerAsync(Convert.ToInt32(singerIdOrName), true).Result;
            }
            else
            {
                //singerToReturn = SingersDataStore.Current.Singers.FirstOrDefault(singer => singer.Name.ToLower().Replace(" ", "") == singerIdOrName.Replace("-", " ").ToLower().Replace(" ", ""));
				singerEntity = _musesRepository.GetSingerAsync(singerIdOrName, true).Result;
            }
			if (singerEntity == null)
            {
                return NotFound();
            }
            else
            {
                if (IsInt(criticismId))
                {
                    //songToReturn = singerToReturn.Songs.FirstOrDefault(song => song.Id == Convert.ToInt32(songIdOrName));
					criticismEntity = singerEntity.Criticisms.FirstOrDefault(c => c.Id == Convert.ToInt32(criticismId));
                }
                else
                {
                    //songToReturn = singerToReturn.Songs.FirstOrDefault(song => song.Name.ToLower().Replace(" ", "") == songIdOrName.Replace("-", " ").ToLower().Replace(" ", ""));
					// criticismEntity = singerEntity.Criticisms.FirstOrDefault(song => song.Name.ToLower().Replace(" ", "") == songIdOrName.Replace("-", " ").ToLower().Replace(" ", ""));
                    return NotFound();
                }
                if (criticismEntity == null)
                {
                    return NotFound();
                }
				var criticismWithoutSingerResult = Mapper.Map<CriticismWithoutSingerDto>(criticismEntity);
				return Ok(criticismWithoutSingerResult);
            }
        }

        [HttpPost("{singerIdOrName}/criticisms")]
        public async Task<IActionResult> PostCriticismOfSinger(string singerIdOrName, [FromBody]CriticismDtoForEdit criticismOfSinger)
        {
            if(criticismOfSinger == null) {
                return BadRequest();
            }
            if (criticismOfSinger.Opinion == criticismOfSinger.Critic)
            {
                ModelState.AddModelError("Description", "The provided Opinion should be diffrent from the Critic");
            }
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

			bool flagSingerExists;
			if (IsInt(singerIdOrName))
            {
				//flagSingerExists = _musesRepository.SingerExistsAsync(Convert.ToInt32(singerIdOrName)).Result;
                flagSingerExists = await _musesRepository.SingerExistsAsync(Convert.ToInt32(singerIdOrName));
            }
            else
            {
				flagSingerExists = await _musesRepository.SingerExistsAsync(singerIdOrName);
            }
			if (!flagSingerExists)
            {
                return NotFound();
            }
            else
            {
				if(await _musesRepository.CriticismExistsAsync(criticismOfSinger))
				{
					_logger.LogWarning("This criticism already exists: " + criticismOfSinger.Critic + "->" + criticismOfSinger.Opinion);
					return BadRequest("This criticism already exists: " + criticismOfSinger.Critic + "->" + criticismOfSinger.Opinion);
				}
				//var maxSongOfSingerId = SingersDataStore.Current.Singers.SelectMany(s => s.Songs).Max(s => s.Id);
				var finalCriticismOfSinger = Mapper.Map<Entities.Criticism>(criticismOfSinger);
				//singerToReturn.Songs.Add(finalSongOfSinger);

				if (IsInt(singerIdOrName))
                {
					await _musesRepository.AddCriticismForSingerAsync(Convert.ToInt32(singerIdOrName), finalCriticismOfSinger);
                }
                else
                {
					await _musesRepository.AddCriticismForSingerAsync(singerIdOrName, finalCriticismOfSinger);
                }


				if(!await _musesRepository.SaveAsync())
				{
					return StatusCode(500, "A problem happened while handling your request.");
				}
				var createdCriticismofSingerToReturn = Mapper.Map<Models.CriticismWithoutSingerDto>(finalCriticismOfSinger);

				return CreatedAtRoute("GetCriticismOfSinger", new { singerIdOrName = singerIdOrName, criticismId = createdCriticismofSingerToReturn.Id }, createdCriticismofSingerToReturn);
            }  

        }

        // GET: api/values
        [HttpGet("{singerIdOrName}/songs")]
        public IActionResult GetSongsOfSinger(string singerIdOrName)
        {
            Singer singerEntity;
            if (IsInt(singerIdOrName))
            {
                singerEntity = _musesRepository.GetSingerAsync(Convert.ToInt32(singerIdOrName), true).Result;
            }
            else
            {
                singerEntity = _musesRepository.GetSingerAsync(singerIdOrName, true).Result;
            }

            if (singerEntity == null)
            {
                return NotFound();
            }
			var songsResult = Mapper.Map<IEnumerable<SongWithoutSingersDto>>(singerEntity.SingerSongs.Select(ss => ss.Song));
			return Ok(songsResult);
        }

        [HttpGet("{singerIdOrName}/songs/{songIdOrName}", Name = "GetSongOfSinger")]
        public IActionResult GetSongOfSinger(string singerIdOrName, string songIdOrName)
        {
			//SingerDto singerToReturn;
			//SongDto songToReturn;
			Singer singerEntity;
			Song songEntity;
            if (IsInt(singerIdOrName))
            {
                //singerToReturn = SingersDataStore.Current.Singers.FirstOrDefault(singer => singer.Id == Convert.ToInt32(singerIdOrName));
				singerEntity = _musesRepository.GetSingerAsync(Convert.ToInt32(singerIdOrName), true).Result;
            }
            else
            {
                //singerToReturn = SingersDataStore.Current.Singers.FirstOrDefault(singer => singer.Name.ToLower().Replace(" ", "") == singerIdOrName.Replace("-", " ").ToLower().Replace(" ", ""));
				singerEntity = _musesRepository.GetSingerAsync(singerIdOrName, true).Result;
            }
			if (singerEntity == null)
            {
                return NotFound();
            }
            else
            {
                if (IsInt(songIdOrName))
                {
                    //songToReturn = singerToReturn.Songs.FirstOrDefault(song => song.Id == Convert.ToInt32(songIdOrName));
					songEntity = singerEntity.Songs.FirstOrDefault(song => song.Id == Convert.ToInt32(songIdOrName));
                }
                else
                {
                    //songToReturn = singerToReturn.Songs.FirstOrDefault(song => song.Name.ToLower().Replace(" ", "") == songIdOrName.Replace("-", " ").ToLower().Replace(" ", ""));
					songEntity = singerEntity.Songs.FirstOrDefault(song => song.Name.ToLower().Replace(" ", "") == songIdOrName.Replace("-", " ").ToLower().Replace(" ", ""));
                }
                if (songEntity == null)
                {
                    return NotFound();
                }
				var songWithoutSingersResult = Mapper.Map<SongWithoutSingersDto>(songEntity);
				return Ok(songWithoutSingersResult);
            }
        }
        // POST api/values
        [HttpPost("{singerIdOrName}/songs")]
        public async Task<IActionResult> PostSongOfSinger(string singerIdOrName, [FromBody]SongDtoForEdit songOfSinger)
        {
            if(songOfSinger == null) {
                return BadRequest();
            }
            if (songOfSinger.Description == songOfSinger.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be diffrent from the name");
            }
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

			//SingerDto singerToReturn;
			//if (IsInt(singerIdOrName))
			//{
			//    singerToReturn = SingersDataStore.Current.Singers.FirstOrDefault(singer => singer.Id == Convert.ToInt32(singerIdOrName));
			//}
			//else
			//{
			//    singerToReturn = SingersDataStore.Current.Singers.FirstOrDefault(singer => singer.Name.ToLower().Replace(" ", "") == singerIdOrName.Replace("-", " ").ToLower().Replace(" ", ""));
			//}

			bool flagSingerExists;
			if (IsInt(singerIdOrName))
            {
				//flagSingerExists = _musesRepository.SingerExistsAsync(Convert.ToInt32(singerIdOrName)).Result;
                flagSingerExists = await _musesRepository.SingerExistsAsync(Convert.ToInt32(singerIdOrName));
            }
            else
            {
				flagSingerExists = await _musesRepository.SingerExistsAsync(singerIdOrName);
            }
			if (!flagSingerExists)
            {
                return NotFound();
            }
            else
            {
				if(await _musesRepository.SongExistsAsync(songOfSinger.Name))
				{
					_logger.LogWarning("This song already exists: " + songOfSinger.Name);
					return BadRequest("This song already exists: " + songOfSinger.Name);
				}
				//var maxSongOfSingerId = SingersDataStore.Current.Singers.SelectMany(s => s.Songs).Max(s => s.Id);
				var finalSongOfSinger = Mapper.Map<Entities.Song>(songOfSinger);
				//singerToReturn.Songs.Add(finalSongOfSinger);

				if (IsInt(singerIdOrName))
                {
					await _musesRepository.AddSongForSingerAsync(Convert.ToInt32(singerIdOrName), finalSongOfSinger);
                }
                else
                {
					await _musesRepository.AddSongForSingerAsync(singerIdOrName, finalSongOfSinger);
                }


				if(!await _musesRepository.SaveAsync())
				{
					return StatusCode(500, "A problem happened while handling your request.");
				}
				var createdSongofSingerToReturn = Mapper.Map<Models.SongWithoutSingersDto>(finalSongOfSinger);

				return CreatedAtRoute("GetSongOfSinger", new { singerIdOrName = singerIdOrName, songIdOrName = createdSongofSingerToReturn.Name }, createdSongofSingerToReturn);
            }  

        }

        // PUT api/values/5
        [HttpPut("{singerIdOrName}/songs/{songIdOrName}")]
        public IActionResult PutSongOfSinger(string singerIdOrName, string songIdOrName, [FromBody]SongDtoForEdit songOfSinger)
        {
            if (songOfSinger == null)
            {
                return BadRequest();
            }
            if (songOfSinger.Description == songOfSinger.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be diffrent from the name");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //SingerDto singerToReturn;
            //if (IsInt(singerIdOrName))
            //{
            //    singerToReturn = SingersDataStore.Current.Singers.FirstOrDefault(singer => singer.Id == Convert.ToInt32(singerIdOrName));
            //}
            //else
            //{
            //    singerToReturn = SingersDataStore.Current.Singers.FirstOrDefault(singer => singer.Name.ToLower().Replace(" ", "") == singerIdOrName.Replace("-", " ").ToLower().Replace(" ", ""));
            //}
            //if (singerToReturn == null)
            //{
            //    return NotFound();
            //}
			bool flagSingerExists;
            if (IsInt(singerIdOrName))
            {
                flagSingerExists = _musesRepository.SingerExistsAsync(Convert.ToInt32(singerIdOrName)).Result;
            }
            else
            {
                flagSingerExists = _musesRepository.SingerExistsAsync(singerIdOrName).Result;
            }
			if (!flagSingerExists)
            {
                return NotFound();
            }
            else
            {
				if (!_musesRepository.SongExistsAsync(songOfSinger.Name).Result)
                {
					return NotFound("This song doesn't exist");
                }
				//SongDto songToReturn;
				//if (IsInt(songIdOrName))
				//{
				//    songToReturn = singerToReturn.Songs.FirstOrDefault(song => song.Id == Convert.ToInt32(songIdOrName));
				//}
				//else
				//{
				//    songToReturn = singerToReturn.Songs.FirstOrDefault(song => song.Name.ToLower().Replace(" ", "") == songIdOrName.Replace("-", " ").ToLower().Replace(" ", ""));
				//}

				//if(songToReturn == null) {
				//    return NotFound();
				//}

				Song songEntity;
				if (IsInt(songIdOrName))
                {
					songEntity = _musesRepository.GetSongAsync(Convert.ToInt32(songIdOrName), false).Result;
                }
                else
                {
					songEntity = _musesRepository.GetSongAsync(songIdOrName, false).Result;
                }
				Mapper.Map(songOfSinger, songEntity);

                //songToReturn.Name = songOfSinger.Name;
                //songToReturn.TranslatedName = songOfSinger.TranslatedName;
                //songToReturn.Author = songOfSinger.Author;
                //songToReturn.Composer = songOfSinger.Composer;
                //songToReturn.Language = songOfSinger.Language;
                //songToReturn.Description = songOfSinger.Description;
				if(!_musesRepository.SaveAsync().Result)
				{
					return StatusCode(500, "A problem happened while handling your request.");
				}
                
                return NoContent(); 
            }  
        }

        // PATCH api/values/5
        [HttpPatch("{singerIdOrName}/songs/{songIdOrName}")]
        public IActionResult PatchSongOfSinger(string singerIdOrName, string songIdOrName, [FromBody]JsonPatchDocument<SongDtoForEdit> patchDoc)
        {
            if(patchDoc == null) {
                return BadRequest();
            }

            //SingerDto singerToReturn;
            //if (IsInt(singerIdOrName))
            //{
            //    singerToReturn = SingersDataStore.Current.Singers.FirstOrDefault(singer => singer.Id == Convert.ToInt32(singerIdOrName));
            //}
            //else
            //{
            //    singerToReturn = SingersDataStore.Current.Singers.FirstOrDefault(singer => singer.Name.ToLower().Replace(" ", "") == singerIdOrName.Replace("-", " ").ToLower().Replace(" ", ""));
            //}
            //if (singerToReturn == null)
            //{
            //    return NotFound();
            //}
			bool flagSingerExists;
            if (IsInt(singerIdOrName))
            {
                flagSingerExists = _musesRepository.SingerExistsAsync(Convert.ToInt32(singerIdOrName)).Result;
            }
            else
            {
                flagSingerExists = _musesRepository.SingerExistsAsync(singerIdOrName).Result;
            }
			if (!flagSingerExists)
            {
                return NotFound();
            }
            else
            {
				bool flagSongExists;
				if (IsInt(songIdOrName))
                {
					flagSongExists = _musesRepository.SongExistsAsync(Convert.ToInt32(songIdOrName)).Result;
                }
                else
                {
					flagSongExists = _musesRepository.SongExistsAsync(songIdOrName).Result;
                }
				if (!flagSongExists)
                {
                    return NotFound("This song doesn't exist");
                }
                //SongDto songToReturn;
                //if (IsInt(songIdOrName))
                //{
                //    songToReturn = singerToReturn.Songs.FirstOrDefault(song => song.Id == Convert.ToInt32(songIdOrName));
                //}
                //else
                //{
                //    songToReturn = singerToReturn.Songs.FirstOrDefault(song => song.Name.ToLower().Replace(" ", "") == songIdOrName.Replace("-", " ").ToLower().Replace(" ", ""));
                //}

                //if(songToReturn == null) {
                //    return NotFound();
                //}

				Song songEntity;
                if (IsInt(songIdOrName))
                {
                    songEntity = _musesRepository.GetSongAsync(Convert.ToInt32(songIdOrName), false).Result;
                }
                else
                {
                    songEntity = _musesRepository.GetSongAsync(songIdOrName, false).Result;
                }
				var songOfSingerToPatch = Mapper.Map<SongDtoForEdit>(songEntity);

                //var songOfSingerToPatch = new SongDtoForEdit()
                //{
                //    Name = songToReturn.Name,
                //    TranslatedName = songToReturn.TranslatedName,
                //    Author = songToReturn.Author,
                //    Composer = songToReturn.Composer,
                //    Language = songToReturn.Language,
                //    Description = songToReturn.Description
                //};

                patchDoc.ApplyTo(songOfSingerToPatch, ModelState);

                if(!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }
            
                // name duplication
				if (_musesRepository.SongExistsAsync(songOfSingerToPatch.Name).Result)
                {
					var songPossibleDupId = _musesRepository.GetSongAsync(songOfSingerToPatch.Name, false).Result.Id;
					if(songPossibleDupId != songEntity.Id)
					{
						return BadRequest("You've named the song into some existing song name: " + songOfSingerToPatch.Name);
					}
                }

                if (songOfSingerToPatch.Description == songOfSingerToPatch.Name) {
                    ModelState.AddModelError("Description", "The provided description should be different from the name.");
                }

                TryValidateModel(songOfSingerToPatch);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Mapper.Map(songOfSingerToPatch, songEntity);

                //songToReturn.Name = songOfSingerToPatch.Name;
                //songToReturn.TranslatedName = songOfSingerToPatch.TranslatedName;
                //songToReturn.Author = songOfSingerToPatch.Author;
                //songToReturn.Composer = songOfSingerToPatch.Composer;
                //songToReturn.Language = songOfSingerToPatch.Language;
                //songToReturn.Description = songOfSingerToPatch.Description;

				if (!_musesRepository.SaveAsync().Result)
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }

                return NoContent(); 
            }  
        }

   //     // DELETE api/values/5
   //     [HttpDelete("{singerIdOrName}/songs/{songIdOrName}")]
   //     public IActionResult DeleteSongOfSinger(string singerIdOrName, string songIdOrName)
   //     {
			//bool flagSingerExists;
   //         if (IsInt(singerIdOrName))
   //         {
   //             flagSingerExists = _musesRepository.SingerExistsAsync(Convert.ToInt32(singerIdOrName)).Result;
   //         }
   //         else
   //         {
   //             flagSingerExists = _musesRepository.SingerExistsAsync(singerIdOrName).Result;
   //         }
			//if (!flagSingerExists)
   //         {
   //             return NotFound();
   //         }
   //         //SingerDto singerToReturn;
   //         //if (IsInt(singerIdOrName))
   //         //{
   //         //    singerToReturn = SingersDataStore.Current.Singers.FirstOrDefault(singer => singer.Id == Convert.ToInt32(singerIdOrName));
   //         //}
   //         //else
   //         //{
   //         //    singerToReturn = SingersDataStore.Current.Singers.FirstOrDefault(singer => singer.Name.ToLower().Replace(" ", "") == singerIdOrName.Replace("-", " ").ToLower().Replace(" ", ""));
   //         //}
   //         //if (singerToReturn == null)
   //         //{
   //         //    return NotFound();
   //         //}
			////if (!_musesRepository.SingerExistsAsync(singerIdOrName).Result)
    //        //{
    //        //    return NotFound();
    //        //}
    //        else
    //        {
				////SongDto songToReturn;
				//bool flagSongExists;
    //            if (IsInt(songIdOrName))
    //            {
    //                //songToReturn = singerToReturn.Songs.FirstOrDefault(song => song.Id == Convert.ToInt32(songIdOrName));
				//	flagSongExists = _musesRepository.SongExistsAsync(Convert.ToInt32(songIdOrName)).Result;
    //            }
    //            else
    //            {
				//	//songToReturn = singerToReturn.Songs.FirstOrDefault(song => song.Name.ToLower().Replace(" ", "") == songIdOrName.Replace("-", " ").ToLower().Replace(" ", ""));
				//	flagSongExists = _musesRepository.SongExistsAsync(songIdOrName).Result;
    //            }
				//if(!flagSongExists) {
				//	return NotFound();
				//}
    //            //if(songToReturn == null) {
    //            //    return NotFound();
    //            //}
				//_musesRepository.DeleteSongAsync()
        //        singerToReturn.Songs.Remove(songToReturn);

        //        return NoContent(); 
        //    }  
        //}

       [HttpDelete("{singerIdOrName}/criticisms/{criticismId}")]
       public async Task<IActionResult> DeleteCriticismOfSinger(string singerIdOrName, string criticismId)
       {
			Singer singerEntity;
			Criticism criticismEntity;
            if (IsInt(singerIdOrName))
            {
                //singerToReturn = SingersDataStore.Current.Singers.FirstOrDefault(singer => singer.Id == Convert.ToInt32(singerIdOrName));
				singerEntity = _musesRepository.GetSingerAsync(Convert.ToInt32(singerIdOrName), true).Result;
            }
            else
            {
                //singerToReturn = SingersDataStore.Current.Singers.FirstOrDefault(singer => singer.Name.ToLower().Replace(" ", "") == singerIdOrName.Replace("-", " ").ToLower().Replace(" ", ""));
				singerEntity = _musesRepository.GetSingerAsync(singerIdOrName, true).Result;
            }
			if (singerEntity == null)
            {
                return NotFound();
            }
            else
            {
                if (IsInt(criticismId))
                {
                    //songToReturn = singerToReturn.Songs.FirstOrDefault(song => song.Id == Convert.ToInt32(songIdOrName));
					criticismEntity = singerEntity.Criticisms.FirstOrDefault(c => c.Id == Convert.ToInt32(criticismId));
                }
                else
                {
                    //songToReturn = singerToReturn.Songs.FirstOrDefault(song => song.Name.ToLower().Replace(" ", "") == songIdOrName.Replace("-", " ").ToLower().Replace(" ", ""));
					// criticismEntity = singerEntity.Criticisms.FirstOrDefault(song => song.Name.ToLower().Replace(" ", "") == songIdOrName.Replace("-", " ").ToLower().Replace(" ", ""));
                    return NotFound();
                }
                if (criticismEntity == null)
                {
                    return NotFound();
                }
                await _musesRepository.DeleteCriticismAsync(criticismEntity);
				// var criticismWithoutSingerResult = Mapper.Map<CriticismWithoutSingerDto>(criticismEntity);
				if(!await _musesRepository.SaveAsync())
				{
					return StatusCode(500, "A problem happened while handling your request.");
				}
                return NoContent(); 
            }  
        }
    }
}
