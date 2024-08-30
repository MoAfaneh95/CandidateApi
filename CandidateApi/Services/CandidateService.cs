using CandidateApi.Models;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using System.Collections.Concurrent;
using System.Formats.Asn1;

namespace CandidateApi.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly string _filePath = string.Empty;
        private readonly ConcurrentDictionary<string, Candidate> _cache = new();
        private readonly IConfiguration _conf;

        public CandidateService(IConfiguration configuration)
        {
            _conf = configuration;
            LoadCandidatesFromCsv();
            _filePath = _conf.GetSection("").Value?? "candidates.csv";
        }

        public Candidate AddOrUpdateCandidate(Candidate candidate)
        {
            string stringUseDB = _conf.GetSection("UseDB").Value;
            bool.TryParse(stringUseDB, out bool useDB);
            if (!useDB)
            {
                _cache[candidate.Email] = candidate;
                SaveCandidatesToCsv();
            }
            else
            {
                // add Code to Save Data in DB
            }
            return candidate;
        }

        public IList<Candidate> AddOrUpdateCandidates(IList<Candidate> candidates)
        {
            string stringUseDB = _conf.GetSection("UseDB").Value;
            bool.TryParse(stringUseDB, out bool useDB);
            if (!useDB)
            {
                foreach (var candidate in candidates)
                {
                    _cache[candidate.Email] = candidate;
                    SaveCandidatesToCsv();
                }
            }
            else
            {
                // add Code to Save Bulk Data in DB
            }
            return candidates;
        }

        public Candidate GetCandidateByEmail(string email)
        {
            _cache.TryGetValue(email, out var candidate);
            return candidate;
        }

        private void LoadCandidatesFromCsv()
        {
            if (!File.Exists(_filePath)) return;

            using var reader = new StreamReader(_filePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
            var candidates = csv.GetRecords<Candidate>().ToList();

            foreach (var candidate in candidates)
            {
                _cache[candidate.Email] = candidate;
            }
        }

        private void SaveCandidatesToCsv()
        {
            using var writer = new StreamWriter(_filePath);
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
            csv.WriteRecords(_cache.Values);
        }
    }
}
