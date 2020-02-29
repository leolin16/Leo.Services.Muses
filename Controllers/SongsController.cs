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
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Leo.Services.Muses.Controllers
{
    [Route("api/[controller]")]
    public class SongsController : Controller
    {
		private IMusesRepository _musesRepository;

		public SongsController(IMusesRepository musesRepository)
		{
			_musesRepository = musesRepository;
		}
		public static bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }
        // GET: api/values
        [HttpGet]
        public IActionResult GetSongs()
        {
            //return Ok(SingersDataStore.Current.Singers);
			var songEntities = _musesRepository.GetSongsAsync().Result;
			var results = Mapper.Map<IEnumerable<SongWithoutSingersDto>>(songEntities);
            return Ok(results);
        }
        // GET: api/values
        [HttpGet("{songIdOrName}")]
		public IActionResult GetSong(string songIdOrName, bool includeSingers = false)
        {
            //SongDto songToReturn;
			Song songEntity;
            if (IsInt(songIdOrName))
            {
                //songToReturn = SingersDataStore.Current.Singers.FirstOrDefault(singer => singer.Id == Convert.ToInt32(singerIdOrName));
				songEntity = _musesRepository.GetSongAsync(Convert.ToInt32(songIdOrName), includeSingers).Result;
            }
            else
            {
                //songToReturn = SingersDataStore.Current.Singers.FirstOrDefault(singer => singer.Name.ToLower().Replace(" ", "") == singerIdOrName.Replace("-", " ").ToLower().Replace(" ", ""));
				songEntity = _musesRepository.GetSongAsync(songIdOrName, includeSingers).Result;
            }
            if (songEntity == null)
            {
                return NotFound();
            }
			if (includeSingers)
            {
                var songResult = Mapper.Map<SongDto>(songEntity);
                return Ok(songResult);
            }
            var songWithoutSingersResult = Mapper.Map<SongWithoutSingersDto>(songEntity);
			return Ok(songWithoutSingersResult);
        }

		// GET: api/values
        [HttpGet("{songIdOrName}/singers")]
        public IActionResult GetSingersOfSong(string songIdOrName)
        {
            Song songEntity;
			if (IsInt(songIdOrName))
            {
                songEntity = _musesRepository.GetSongAsync(Convert.ToInt32(songIdOrName), true).Result;
            }
            else
            {
                songEntity = _musesRepository.GetSongAsync(songIdOrName, true).Result;
            }

            if (songEntity == null)
            {
                return NotFound();
            }
            var singersResult = Mapper.Map<IEnumerable<SingerWithoutSongsDto>>(songEntity.SingerSongs.Select(ss => ss.Singer));
			return Ok(singersResult);
        }

		[HttpGet("{songIdOrName}/singers/{singerIdOrName}", Name = "GetSingerOfSong")]
		public IActionResult GetSingerOfSong(string songIdOrName, string singerIdOrName)
		{
            Song songEntity;
            Singer singerEntity;
            if (IsInt(songIdOrName))
            {
                //singerToReturn = SingersDataStore.Current.Singers.FirstOrDefault(singer => singer.Id == Convert.ToInt32(singerIdOrName));
				songEntity = _musesRepository.GetSongAsync(Convert.ToInt32(songIdOrName), true).Result;
            }
            else
            {
                //singerToReturn = SingersDataStore.Current.Singers.FirstOrDefault(singer => singer.Name.ToLower().Replace(" ", "") == singerIdOrName.Replace("-", " ").ToLower().Replace(" ", ""));
				songEntity = _musesRepository.GetSongAsync(songIdOrName, true).Result;
            }
            if (songEntity == null)
            {
                return NotFound();
            }
            else
            {
                if (IsInt(singerIdOrName))
                {
                    singerEntity = songEntity.Singers.FirstOrDefault(singer => singer.Id == Convert.ToInt32(singerIdOrName));
                }
                else
                {
					singerEntity = songEntity.Singers.FirstOrDefault(singer => singer.Name.ToLower().Replace(" ", "") == singerIdOrName.Replace("-", " ").ToLower().Replace(" ", ""));
                }
                if (singerEntity == null)
                {
                    return NotFound();
                }
                var singerWithoutSongsResult = Mapper.Map<SingerWithoutSongsDto>(singerEntity);
				return Ok(singerWithoutSongsResult);
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{songIdOrName}")]
		public IActionResult DeleteSong(string songIdOrName)
        {
			bool flagSongExists;
			if(IsInt(songIdOrName)) {
				flagSongExists = _musesRepository.SongExistsAsync(Convert.ToInt32(songIdOrName)).Result;
			} else {
				flagSongExists = _musesRepository.SongExistsAsync(songIdOrName).Result;
			}
			if(!flagSongExists) {
				return NotFound();
			}

			Song songEntity;
            if (IsInt(songIdOrName))
            {
                songEntity = _musesRepository.GetSongAsync(Convert.ToInt32(songIdOrName), false).Result;
            }
            else
            {
                songEntity = _musesRepository.GetSongAsync(songIdOrName, false).Result;
            }
			_musesRepository.DeleteSongAsync(songEntity);
			if(!_musesRepository.SaveAsync().Result) {
				return StatusCode(500, "A problem happened while handling your request.");
			}
			return NoContent();
        }
    }
}
