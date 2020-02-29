using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Leo.Services.Muses.Data;
using Leo.Services.Muses.Entities;
using Leo.Services.Muses.Interfaces;
using Leo.Services.Muses.Models;
using Microsoft.EntityFrameworkCore;

namespace Leo.Services.Muses.Repositories
{
	public class MusesRepository : IMusesRepository
    {
		private MusesDbContext _context;

		public MusesRepository(MusesDbContext context)
        {
			_context = context;
        }

		public Task AddSongAsync(Song song)
		{
			throw new NotImplementedException();
		}

		public async Task AddSongForSingerAsync(int singerId, Song song)
		{
			var singer = GetSingerAsync(singerId, false).Result;
			var singerSong = new SingerSong { Singer = singer, Song = song };
			await _context.AddAsync(singerSong);
            
		}

		public async Task AddSongForSingerAsync(string singerName, Song song)
		{
			var singer = GetSingerAsync(singerName, false).Result;
            var singerSong = new SingerSong { Singer = singer, Song = song };
            await _context.AddAsync(singerSong);
		}

        public async Task DeleteSongAsync(Song song)
		{
			//var songWithRelations = song.
			await Task.FromResult(_context.Remove(song));
		}

		public async Task<Singer> GetSingerAsync(int id, bool includeSongs)
		{
			if(includeSongs) {
				return await _context.Singers.Include(s => s.SingerSongs).ThenInclude(ss => ss.Song).Where(s => s.Id == id).FirstOrDefaultAsync();
			} else {
				return await _context.Singers.Where(s => s.Id == id).FirstOrDefaultAsync();
			}
		}

		public async Task<Singer> GetSingerAsync(string name, bool includeSongs)
        {
            if (includeSongs)
            {
				return await _context.Singers.Include(s => s.SingerSongs).ThenInclude(ss => ss.Song).Where(s => s.Name.ToLower().Replace(" ", "") == name.Replace("-", " ").ToLower().Replace(" ", "")).FirstOrDefaultAsync();
            }
            else
            {
				return await _context.Singers.Where(s => s.Name.ToLower().Replace(" ", "") == name.Replace("-", " ").ToLower().Replace(" ", "")).FirstOrDefaultAsync();
            }

        }

		public async Task<IEnumerable<Singer>> GetSingersAsync()
		{
			return await _context.Singers.OrderBy(s => s.Name).ToListAsync();
		}
        

		public async Task<Song> GetSongAsync(int id, bool includeSingers)
		{
			if (includeSingers)
            {
                return await _context.Songs.Include(s => s.SingerSongs).ThenInclude(ss => ss.Singer).Where(s => s.Id == id).FirstOrDefaultAsync();
            }
            else
            {
                return await _context.Songs.Where(s => s.Id == id).FirstOrDefaultAsync();
            }
		}

        public async Task<Song> GetSongAsync(string name, bool includeSingers)
        {
            if (includeSingers)
            {
                return await _context.Songs.Include(s => s.SingerSongs).ThenInclude(ss => ss.Singer).Where(s => s.Name.ToLower().Replace(" ", "") == name.Replace("-", " ").ToLower().Replace(" ", "")).FirstOrDefaultAsync();
            }
            else
            {
                return await _context.Songs.Where(s => s.Name.ToLower().Replace(" ", "") == name.Replace("-", " ").ToLower().Replace(" ", "")).FirstOrDefaultAsync();
            }

        }

		public async Task<IEnumerable<Song>> GetSongsAsync()
		{
			return await _context.Songs.OrderBy(s => s.Name).ToListAsync();
		}

		public async Task<bool> SaveAsync()
		{
			return await Task.FromResult(_context.SaveChangesAsync().Result >= 0);
		}

		public async Task<bool> SingerExistsAsync(int id)
		{
			return await _context.Singers.AnyAsync(s => s.Id == id);
		}

		public async Task<bool> SingerExistsAsync(string name)
		{
			return await _context.Singers.AnyAsync(s => s.Name.ToLower().Replace(" ", "") == name.Replace("-", " ").ToLower().Replace(" ", ""));
		}

		public async Task<bool> SongExistsAsync(int id)
		{
			return await _context.Songs.AnyAsync(s => s.Id == id);
		}

		public async Task<bool> SongExistsAsync(string name)
		{
			return await _context.Songs.AnyAsync(s => s.Name.ToLower().Replace(" ", "") == name.Replace("-", " ").ToLower().Replace(" ", ""));
		}

        public async Task<bool> CriticismExistsAsync(int id)
        {
            return await _context.Criticisms.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CriticismExistsAsync(CriticismDtoForEdit criticism)
        {
            return await _context.Criticisms.AnyAsync(c => 
					c.Critic.ToLower().Replace(" ", "") == criticism.Critic.ToLower().Replace(" ", "") 
				&&  c.Opinion.ToLower().Replace(" ", "") == criticism.Opinion.ToLower().Replace(" ", "") );
        }

        public async Task AddCriticismForSingerAsync(int singerId, Criticism criticism)
        {
			// // method1: to add subelement into the picked out object
			// var singer = await GetSingerAsync(singerId, false);
			// singer.Criticisms.Add(criticism);
			// // method2: to directly add subelement into dbset of context with a foreign key set
			criticism.SingerId = singerId;
			await _context.Criticisms.AddAsync(criticism);
        }

        public async Task AddCriticismForSingerAsync(string singerName, Criticism criticism)
        {
			var singer = await GetSingerAsync(singerName, false);
            singer.Criticisms.Add(criticism);
			// criticism.SingerId = singer.Id;
			// await _context.Criticisms.AddAsync(criticism);
        }
        public async Task DeleteCriticismAsync(Criticism criticism)
		{
			//var songWithRelations = song.
			await Task.FromResult(_context.Remove(criticism));
		}
    }
}
