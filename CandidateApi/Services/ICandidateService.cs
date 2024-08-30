using CandidateApi.Models;

namespace CandidateApi.Services
{
    public interface ICandidateService
    {
        Candidate AddOrUpdateCandidate(Candidate candidate);
        IList<Candidate> AddOrUpdateCandidates(IList<Candidate> candidates);
        Candidate GetCandidateByEmail(string email);
    }
}
