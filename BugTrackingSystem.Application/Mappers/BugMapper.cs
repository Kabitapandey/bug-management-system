using BugTrackingSystem.Application.DTOs;
using BugTrackingSystem.Domain.Entities;

namespace BugTrackingSystem.Application.Mappers
{
    public class BugMapper
    {
        public BugDTOs ConvertToDTO(Bug bug)
        {
            BugDTOs bugDTOs = new BugDTOs();

            bugDTOs.BugId = bug.BugId;
            bugDTOs.Title = bug.Title;
            bugDTOs.Description = bug.Description;
            bugDTOs.Severity = bug.Severity;
            bugDTOs.ReproductionSteps = bug.ReproductionSteps;
            bugDTOs.IsAssigned = bug.IsAssigned;
            bugDTOs.CreatedOn = bug.CreatedAt;
            bugDTOs.Status = bug.Status;

            return bugDTOs;
        }

        public Bug ConvertToEntity(BugDTOs bugDTOs)
        {
            Bug bug = new Bug();

            bug.Title = bugDTOs.Title;
            bug.Description = bugDTOs.Description;
            bug.Severity = bugDTOs.Severity;
            bug.ReproductionSteps = bugDTOs.ReproductionSteps;
            bug.Status = bugDTOs.Status;

            return bug;
        }
    }
}
