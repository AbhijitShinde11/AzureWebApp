using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using StorageAccounts.Mvc.Data;
using StorageAccounts.Mvc.Services;

namespace StorageAccounts.Mvc.Controllers
{
    public class AttendeeController(ITableStorageService tableStorageService, IBlobStorageService blobStorageService, IQueueService queueService) : Controller
    {
        private readonly ITableStorageService _tableStorageService = tableStorageService;
        private readonly IBlobStorageService _blobStorageService = blobStorageService;
        private readonly IQueueService _queueService = queueService;

        public async Task<ActionResult> Index()
        {
            var data = await _tableStorageService.GetAttendees();
            foreach (var image in data)
            {
                image.ImageName = await _blobStorageService.GetBlobUrl(image.ImageName);
            }
            return View(data);
        }

        // GET: AttendeeRegistrationController/Details/5
        public async Task<ActionResult> Details(string id, string industry)
        {
            var data = await _tableStorageService.GetAttendee(industry, id);
            data.ImageName = await _blobStorageService.GetBlobUrl(data.ImageName);
            return View(data);
        }

        // GET: AttendeeRegistrationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AttendeeRegistrationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Attendee attendeeEntity, 
            IFormFile formFile)
        {
            try
            {
                var id = Guid.NewGuid().ToString();
                attendeeEntity.PartitionKey = attendeeEntity.Industry;
                attendeeEntity.RowKey = id;

                if (formFile?.Length > 0)
                {
                    attendeeEntity.ImageName =
                        await _blobStorageService.UploadBlob(formFile, id);
                }
                else
                {
                    attendeeEntity.ImageName = "default.jpg";
                }

                await _tableStorageService.UpsertAttendee(attendeeEntity);

                var email = new MailMessage
                {
                    From = new MailAddress(attendeeEntity.Email),
                    Subject = "Attendee Created",
                    Body = $"Hello {attendeeEntity.FirstName} {attendeeEntity.LastName}," +
                    $"\n\r Thank you for registering for this event. " +
                    $"\n\r Your record has been saved for future reference. "
                };
                await _queueService.SendMessage(email);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AttendeeRegistrationController/Edit/5
        public async Task<ActionResult> Edit(string id, string industry)
        {
            var data = await _tableStorageService.GetAttendee(industry, id);

            return View(data);
        }

        // POST: AttendeeRegistrationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Attendee attendeeEntity,
            IFormFile formFile)
        {
            try
            {
                if (formFile?.Length > 0)
                {
                    attendeeEntity.ImageName = await _blobStorageService.UploadBlob(formFile, attendeeEntity.RowKey, attendeeEntity.ImageName);
                }

                attendeeEntity.PartitionKey = attendeeEntity.Industry;
                
                await _tableStorageService.UpsertAttendee(attendeeEntity);

                var email = new MailMessage
                {
                    From = new MailAddress(attendeeEntity.Email),
                    Subject = "Attendee Updated",
                    Body = $"Hello {attendeeEntity.FirstName} {attendeeEntity.LastName}," +
                    $"\n\r Thank you for registering for this event. " +
                    $"\n\r Your record has been saved for future reference. "
                };

                await _queueService.SendMessage(email);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: AttendeeRegistrationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, string industry)
        {
            try
            {
                var data = await _tableStorageService.GetAttendee(industry, id);
                await _tableStorageService.DeleteAttendee(industry, id);
                await _blobStorageService.RemoveBlob(data.ImageName);

                var email = new MailMessage
                {
                    From = new MailAddress(data.Email),
                    Subject = "Attendee Deleted",
                    Body = $"Hello {data.FirstName} {data.LastName}," +
                    $"\n\r Thank you for registering for this event. " +
                    $"\n\r Your record has been saved for future reference. "
                };

                await _queueService.SendMessage(email);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}