using BuildingCompanyApp.Models;

namespace BuildingCompanyApp.Services
{
    public static class AppDataStore
    {
        public static List<User> Users { get; } =
        [
            CreateUser(1, "admin", "admin@qore.am", "Qore", "Admin", "Admin", "admin123"),
            CreateUser(2, "karen.manager", "karen@qore.am", "Karen", "Sargsyan", "Manager", "karen123"),
            CreateUser(3, "arman.engineer", "arman@qore.am", "Arman", "Hakobyan", "Employee", "arman123"),
            CreateUser(4, "mariam.architect", "mariam@qore.am", "Mariam", "Petrosyan", "Employee", "mariam123"),
            CreateUser(5, "suren.worker", "suren@qore.am", "Suren", "Grigoryan", "Employee", "suren123"),
            CreateUser(6, "lilit.accountant", "lilit@qore.am", "Lilit", "Gevorgyan", "Employee", "lilit123")
        ];

        public static List<Worker> Workers { get; } =
        [
            CreateWorker(1, 3, "Arman Hakobyan", "Site Engineer", "Engineer", "Concrete and structural supervision", 520000, "Present", 9.5m, "+374 91 123456", "Yerevan, Ajapnyak"),
            CreateWorker(2, 4, "Mariam Petrosyan", "Lead Architect", "Architect", "Residential planning and facade packages", 610000, "On Site", 8m, "+374 93 987654", "Yerevan, Davtashen"),
            CreateWorker(3, 2, "Karen Sargsyan", "Foreman", "Manager", "Crew coordination and task planning", 470000, "Present", 10m, "+374 94 334455", "Abovyan"),
            CreateWorker(4, 5, "Suren Grigoryan", "General Worker", "Worker", "Masonry and reinforcement support", 290000, "Present", 8.5m, "+374 95 112233", "Artashat"),
            CreateWorker(5, 6, "Lilit Gevorgyan", "Accounting Specialist", "Accountant", "Payroll and vendor payments", 430000, "Remote", 7.5m, "+374 96 889900", "Yerevan, Arabkir")
        ];

        public static List<Project> Projects { get; } =
        [
            CreateProject(1, "North Residence", "Premium residential building with underground parking and rooftop terrace.", "In Progress", "Bagrevand 14, Yerevan", "Narek Development", "hello@narekdev.am", "+374 10 554433", "Walls", 42, "Karen Sargsyan", "High", 185000000, DateTime.Today.AddMonths(-3), DateTime.Today.AddMonths(8), 1, "north-residence"),
            CreateProject(2, "Sevan Villa Complex", "Lakeside villas with custom interiors, landscaping and utility upgrades.", "Planning", "Sevan, Gegharkunik", "Apex Living", "procurement@apexliving.am", "+374 55 667788", "Foundation", 15, "Arman Hakobyan", "Medium", 92000000, DateTime.Today.AddDays(-20), DateTime.Today.AddMonths(10), 0, "sevan-villa"),
            CreateProject(3, "Cascade Office Fit-Out", "Interior reconstruction, MEP refresh and premium meeting rooms.", "Completed", "Tamanyan 3, Yerevan", "Vertex Capital", "ops@vertex.am", "+374 44 223344", "Finished", 100, "Mariam Petrosyan", "High", 47000000, DateTime.Today.AddMonths(-7), DateTime.Today.AddMonths(-1), 5, "cascade-office")
        ];

        public static List<Document> Documents { get; } =
        [
            new Document { DocumentId = 1, ProjectId = 1, DocumentName = "North Residence Permit Pack", DocumentPath = "#", DocumentType = "PDF", Category = "Permit", Tags = "permit,municipality,foundation", FileSizeInBytes = 2_400_000, UploadedBy = 2, UploadedDate = DateTime.Today.AddDays(-18) },
            new Document { DocumentId = 2, ProjectId = 1, DocumentName = "Structural Wall Drawings", DocumentPath = "#", DocumentType = "Drawing", Category = "Blueprint", Tags = "cad,walls,structural", FileSizeInBytes = 6_800_000, UploadedBy = 4, UploadedDate = DateTime.Today.AddDays(-12) },
            new Document { DocumentId = 3, ProjectId = 3, DocumentName = "Client Contract - Vertex Capital", DocumentPath = "#", DocumentType = "Contract", Category = "Contract", Tags = "contract,client,invoice", FileSizeInBytes = 1_900_000, UploadedBy = 6, UploadedDate = DateTime.Today.AddMonths(-3), ExpiryDate = DateTime.Today.AddYears(1) }
        ];

        public static List<MaterialItem> Materials { get; } =
        [
            CreateMaterial(1, "Cement", "bags", 480, 3850, 150, "Ararat Supplies", "Warehouse A"),
            CreateMaterial(2, "Sand", "tons", 82, 12000, 25, "ShinMarket", "Outdoor Yard"),
            CreateMaterial(3, "Metal", "tons", 14, 420000, 8, "SteelHub", "Warehouse B"),
            CreateMaterial(4, "Paint", "buckets", 24, 18500, 18, "ColorMix", "Finishing Shelf"),
            CreateMaterial(5, "Tiles", "sqm", 310, 7600, 120, "Tile House", "Warehouse C")
        ];

        public static List<MaterialTransaction> MaterialTransactions { get; } =
        [
            new MaterialTransaction { MaterialTransactionId = 1, MaterialItemId = 1, TransactionType = "IN", Quantity = 200, UnitPrice = 3800, SupplierName = "Ararat Supplies", ReferenceProject = "North Residence", TransactionDate = DateTime.Today.AddDays(-9) },
            new MaterialTransaction { MaterialTransactionId = 2, MaterialItemId = 3, TransactionType = "OUT", Quantity = 6, UnitPrice = 420000, SupplierName = "SteelHub", ReferenceProject = "North Residence", TransactionDate = DateTime.Today.AddDays(-2) },
            new MaterialTransaction { MaterialTransactionId = 3, MaterialItemId = 4, TransactionType = "OUT", Quantity = 8, UnitPrice = 18500, SupplierName = "ColorMix", ReferenceProject = "Cascade Office Fit-Out", TransactionDate = DateTime.Today.AddDays(-1) }
        ];

        public static List<FinanceEntry> FinanceEntries { get; } =
        [
            new FinanceEntry { FinanceEntryId = 1, EntryType = "Income", Category = "Client Payment", ProjectName = "North Residence", Amount = 18500000, PaymentMethod = "Bank Transfer", Notes = "Milestone payment for structural phase.", EntryDate = DateTime.Today.AddDays(-7) },
            new FinanceEntry { FinanceEntryId = 2, EntryType = "Expense", Category = "Payroll", ProjectName = "North Residence", WorkerId = 1, Amount = 520000, PaymentMethod = "Bank Transfer", Notes = "Monthly salary for Arman Hakobyan.", EntryDate = DateTime.Today.AddDays(-5) },
            new FinanceEntry { FinanceEntryId = 3, EntryType = "Expense", Category = "Payroll", ProjectName = "North Residence", WorkerId = 3, Amount = 470000, PaymentMethod = "Bank Transfer", Notes = "Monthly salary for Karen Sargsyan.", EntryDate = DateTime.Today.AddDays(-5) },
            new FinanceEntry { FinanceEntryId = 4, EntryType = "Expense", Category = "Payroll", ProjectName = "Cascade Office Fit-Out", WorkerId = 2, Amount = 610000, PaymentMethod = "Bank Transfer", Notes = "Monthly salary for Mariam Petrosyan.", EntryDate = DateTime.Today.AddDays(-5) },
            new FinanceEntry { FinanceEntryId = 5, EntryType = "Expense", Category = "Payroll", ProjectName = "North Residence", WorkerId = 4, Amount = 290000, PaymentMethod = "Bank Transfer", Notes = "Monthly salary for Suren Grigoryan.", EntryDate = DateTime.Today.AddDays(-5) },
            new FinanceEntry { FinanceEntryId = 6, EntryType = "Expense", Category = "Payroll", ProjectName = "Qore HQ", WorkerId = 5, Amount = 430000, PaymentMethod = "Bank Transfer", Notes = "Monthly salary for Lilit Gevorgyan.", EntryDate = DateTime.Today.AddDays(-5) },
            new FinanceEntry { FinanceEntryId = 7, EntryType = "Expense", Category = "Materials", ProjectName = "North Residence", Amount = 4640000, PaymentMethod = "Supplier Credit", Notes = "Rebar and cement procurement.", EntryDate = DateTime.Today.AddDays(-3) },
            new FinanceEntry { FinanceEntryId = 8, EntryType = "Income", Category = "Interior Advance", ProjectName = "Cascade Office Fit-Out", Amount = 6200000, PaymentMethod = "Card", Notes = "Final release against handover package.", EntryDate = DateTime.Today.AddDays(-2) }
        ];

        public static List<Invoice> Invoices { get; } =
        [
            new Invoice { InvoiceId = 1, InvoiceNumber = "INV-2026-041", ClientName = "Narek Development", ProjectName = "North Residence", Amount = 9200000, Status = "Pending", DueDate = DateTime.Today.AddDays(4) },
            new Invoice { InvoiceId = 2, InvoiceNumber = "INV-2026-036", ClientName = "Vertex Capital", ProjectName = "Cascade Office Fit-Out", Amount = 4700000, Status = "Paid", DueDate = DateTime.Today.AddDays(-15), PaidDate = DateTime.Today.AddDays(-10) }
        ];

        public static List<Client> Clients { get; } =
        [
            new Client { ClientId = 1, Name = "Narek Harutyunyan", CompanyName = "Narek Development", Email = "hello@narekdev.am", Phone = "+374 10 554433", ContractStatus = "Signed", ProjectHistory = "North Residence, facade extension review", Notes = "Wants weekly photo updates and permit follow-up notes.", ReminderDate = DateTime.Today.AddDays(1), LastContactDate = DateTime.Today.AddDays(-2) },
            new Client { ClientId = 2, Name = "Anna Mkrtchyan", CompanyName = "Apex Living", Email = "procurement@apexliving.am", Phone = "+374 55 667788", ContractStatus = "Negotiation", ProjectHistory = "Sevan Villa Complex pre-construction meetings", Notes = "Needs cost breakdown before final signature.", ReminderDate = DateTime.Today.AddDays(3), LastContactDate = DateTime.Today.AddDays(-1) },
            new Client { ClientId = 3, Name = "David Vardanyan", CompanyName = "Vertex Capital", Email = "ops@vertex.am", Phone = "+374 44 223344", ContractStatus = "Closed", ProjectHistory = "Cascade Office Fit-Out delivered in full", Notes = "Potential referral for a second office floor.", ReminderDate = DateTime.Today.AddDays(10), LastContactDate = DateTime.Today.AddDays(-6) }
        ];

        public static List<WorkTask> Tasks { get; } =
        [
            new WorkTask { WorkTaskId = 1, ProjectId = 1, AssignedWorkerId = 3, CreatedByUserId = 1, Title = "Finish 2nd floor concrete by Friday", Description = "Coordinate rebar delivery, pour schedule and site inspection.", ProjectName = "North Residence", AssignedTo = "Karen Sargsyan", Priority = "High", Status = "In Progress", ProgressPercent = 68, CommentCount = 6, LatestComment = "Pump truck confirmed for Thursday morning.", NotificationsEnabled = true, CreatedDate = DateTime.Today.AddDays(-4), DueDate = DateTime.Today.AddDays(2) },
            new WorkTask { WorkTaskId = 2, ProjectId = 2, AssignedWorkerId = 2, CreatedByUserId = 2, Title = "Approve villa foundation drawings", Description = "Finalize revised structural package with architect comments.", ProjectName = "Sevan Villa Complex", AssignedTo = "Mariam Petrosyan", Priority = "Medium", Status = "Review", ProgressPercent = 45, CommentCount = 3, LatestComment = "Pending client sign-off on retaining wall note.", NotificationsEnabled = true, CreatedDate = DateTime.Today.AddDays(-3), DueDate = DateTime.Today.AddDays(5) },
            new WorkTask { WorkTaskId = 3, ProjectId = 3, AssignedWorkerId = 5, CreatedByUserId = 1, Title = "Collect signed handover documents", Description = "Upload client acceptance forms and final invoice copy.", ProjectName = "Cascade Office Fit-Out", AssignedTo = "Lilit Gevorgyan", Priority = "Low", Status = "Open", ProgressPercent = 20, CommentCount = 1, LatestComment = "Waiting for one more stamped invoice copy.", NotificationsEnabled = false, CreatedDate = DateTime.Today.AddDays(-2), DueDate = DateTime.Today.AddDays(1) },
            new WorkTask { WorkTaskId = 4, ProjectId = 1, AssignedWorkerId = 4, CreatedByUserId = 2, Title = "Prepare wall reinforcement area", Description = "Stage tools and clear the next pour zone for the wall crew.", ProjectName = "North Residence", AssignedTo = "Suren Grigoryan", Priority = "Medium", Status = "Open", ProgressPercent = 10, CommentCount = 0, LatestComment = "New assignment.", NotificationsEnabled = true, CreatedDate = DateTime.Today.AddDays(-1), DueDate = DateTime.Today.AddDays(2) }
        ];

        static AppDataStore()
        {
            LinkUsersAndWorkers();
            AssignWorkersToProjects();
        }

        private static void LinkUsersAndWorkers()
        {
            foreach (var worker in Workers)
            {
                var user = Users.FirstOrDefault(u => u.UserId == worker.UserId);
                if (user == null)
                {
                    continue;
                }

                worker.User = user;
                user.Worker = worker;
            }
        }

        private static void AssignWorkersToProjects()
        {
            AttachWorkerToProject(1, 1, "Structural Engineer");
            AttachWorkerToProject(1, 3, "Site Manager");
            AttachWorkerToProject(1, 4, "Crew Worker");
            AttachWorkerToProject(2, 1, "Engineer");
            AttachWorkerToProject(2, 2, "Architect");
            AttachWorkerToProject(3, 2, "Lead Architect");
            AttachWorkerToProject(3, 5, "Finance Coordinator");
        }

        public static void AttachWorkerToProject(int projectId, int workerId, string role)
        {
            var project = Projects.FirstOrDefault(p => p.ProjectId == projectId);
            var worker = Workers.FirstOrDefault(w => w.WorkerId == workerId);
            if (project == null || worker == null)
            {
                return;
            }

            if (project.ProjectWorkers.Any(link => link.WorkerId == workerId))
            {
                return;
            }

            var projectWorker = new ProjectWorker
            {
                ProjectWorkerId = ProjectWorkersNextId(),
                ProjectId = projectId,
                WorkerId = workerId,
                Role = role,
                AssignedDate = DateTime.Today.AddDays(-14),
                Project = project,
                Worker = worker
            };

            project.ProjectWorkers.Add(projectWorker);
            worker.ProjectWorkers.Add(projectWorker);
        }

        public static void ReplaceProjectAssignments(int projectId, IEnumerable<int> workerIds)
        {
            var project = Projects.FirstOrDefault(p => p.ProjectId == projectId);
            if (project == null)
            {
                return;
            }

            foreach (var worker in Workers)
            {
                var links = worker.ProjectWorkers.Where(link => link.ProjectId == projectId).ToList();
                foreach (var link in links)
                {
                    worker.ProjectWorkers.Remove(link);
                }
            }

            project.ProjectWorkers.Clear();

            foreach (var workerId in workerIds.Distinct())
            {
                var worker = Workers.FirstOrDefault(w => w.WorkerId == workerId);
                if (worker == null)
                {
                    continue;
                }

                AttachWorkerToProject(projectId, workerId, worker.Role);
            }
        }

        private static int ProjectWorkersNextId()
        {
            return Projects.SelectMany(project => project.ProjectWorkers).DefaultIfEmpty().Max(link => link?.ProjectWorkerId ?? 0) + 1;
        }

        private static User CreateUser(int id, string username, string email, string firstName, string lastName, string role, string password)
        {
            return new User
            {
                UserId = id,
                Username = username,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = role,
                CreatedDate = DateTime.UtcNow.AddMonths(-6),
                IsActive = true
            };
        }

        private static Worker CreateWorker(int workerId, int userId, string fullName, string position, string role, string specialization, decimal salary, string attendanceStatus, decimal workHours, string phone, string address)
        {
            return new Worker
            {
                WorkerId = workerId,
                UserId = userId,
                FullName = fullName,
                Position = position,
                Role = role,
                Specialization = specialization,
                Salary = salary,
                AttendanceStatus = attendanceStatus,
                WorkHours = workHours,
                Phone = phone,
                Address = address,
                HireDate = DateTime.Today.AddYears(-2),
                CreatedDate = DateTime.UtcNow.AddMonths(-6)
            };
        }

        private static Project CreateProject(int id, string name, string description, string status, string location, string clientName, string clientEmail, string clientPhone, string currentStage, int stagePercent, string siteManager, string priority, decimal budget, DateTime startDate, DateTime deadline, int completedStages, string imageSeed)
        {
            var project = new Project
            {
                ProjectId = id,
                ProjectName = name,
                Description = description,
                Status = status,
                Location = location,
                ClientName = clientName,
                ClientEmail = clientEmail,
                ClientPhone = clientPhone,
                CurrentStage = currentStage,
                StageCompletionPercent = stagePercent,
                SiteManager = siteManager,
                Priority = priority,
                Budget = budget,
                StartDate = startDate,
                Deadline = deadline,
                CreatedDate = startDate,
                UpdatedDate = DateTime.UtcNow
            };

            var stageNames = new[] { "Foundation", "Walls", "Roof", "Interior", "Finished" };
            for (var i = 0; i < stageNames.Length; i++)
            {
                project.Stages.Add(new ProjectStage
                {
                    ProjectStageId = (id * 10) + i,
                    ProjectId = id,
                    StageName = stageNames[i],
                    DisplayOrder = i + 1,
                    IsCompleted = i < completedStages
                });
            }

            project.ProjectImages.Add(new ProjectImage
            {
                ImageId = id,
                ProjectId = id,
                ImageName = $"{name} preview",
                ImageUrl = $"https://picsum.photos/seed/{imageSeed}/640/420",
                UploadedDate = DateTime.Today.AddDays(-4)
            });

            return project;
        }

        private static MaterialItem CreateMaterial(int id, string name, string unit, decimal quantityInStock, decimal unitPrice, decimal reorderLevel, string supplierName, string storageLocation)
        {
            return new MaterialItem
            {
                MaterialItemId = id,
                Name = name,
                Unit = unit,
                QuantityInStock = quantityInStock,
                UnitPrice = unitPrice,
                ReorderLevel = reorderLevel,
                SupplierName = supplierName,
                StorageLocation = storageLocation,
                LastUpdated = DateTime.Today.AddDays(-1)
            };
        }
    }
}
