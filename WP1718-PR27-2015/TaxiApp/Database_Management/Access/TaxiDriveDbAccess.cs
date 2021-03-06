﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaxiApp.Common;
using TaxiApp.Models;
using System.Data.Entity;

namespace TaxiApp.Database_Management.Access
{
    public class TaxiDriveDbAccess : BaseDbAccess<TaxiDrive, string>
    {
        #region Instance
        private static TaxiDriveDbAccess _instance;
        private static readonly object _syncLock = new object();
        public static TaxiDriveDbAccess Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new TaxiDriveDbAccess();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        public override bool Add(TaxiDrive entityToAdd)
        {
            bool result = false;

            using (TaxiDbContext db = new TaxiDbContext())
            {
                if (!db.TaxiDrives.Any(td => td.TaxiDriveID.Equals(entityToAdd.TaxiDriveID)))
                {
                    try
                    {
                        db.TaxiDrives.Add(entityToAdd);
                        db.SaveChanges();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }

            return result;
        }

        public override bool Modify(TaxiDrive entityToModify)
        {
            bool result = false;

            using (TaxiDbContext db = new TaxiDbContext())
            {
                if (db.TaxiDrives.Any(td => td.TaxiDriveID.Equals(entityToModify.TaxiDriveID)))
                {
                    try
                    {
                        TaxiDrive foundTaxiDrive = db.TaxiDrives.Include(td => td.TaxiDriveDriver)
                                                                .Include(td => td.TaxiDriveCustomer)
                                                                .Include(td => td.TaxiDriveDispatcher)
                                                                .Include(td => td.TaxiDriveComment)
                                                                .Include(td => td.TaxiDriveStartingLocation)
                                                                .Include(td => td.TaxiDriveDestination)
                                                                .SingleOrDefault(td => td.TaxiDriveID.Equals(entityToModify.TaxiDriveID));
                        db.TaxiDrives.Attach(foundTaxiDrive);

                        foundTaxiDrive.VehicleType = entityToModify.VehicleType;
                        foundTaxiDrive.DriveStatus = entityToModify.DriveStatus;
                        foundTaxiDrive.Amount = entityToModify.Amount;
                        foundTaxiDrive.TaxiDriveDriver = entityToModify.TaxiDriveDriver;
                        foundTaxiDrive.TaxiDriveCustomer = entityToModify.TaxiDriveCustomer;
                        foundTaxiDrive.TaxiDriveDispatcher = entityToModify.TaxiDriveDispatcher;
                        foundTaxiDrive.TaxiDriveComment = entityToModify.TaxiDriveComment;
                        foundTaxiDrive.TaxiDriveStartingLocation = entityToModify.TaxiDriveStartingLocation;
                        foundTaxiDrive.TaxiDriveDestination = entityToModify.TaxiDriveDestination;

                        db.SaveChanges();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }

            return result;
        }

        public override bool Delete(string entityToDeleteID)
        {
            bool result = false;

            using (TaxiDbContext db = new TaxiDbContext())
            {
                if (db.TaxiDrives.Any(td => td.TaxiDriveID.Equals(entityToDeleteID)))
                {
                    try
                    {
                        TaxiDrive entityToDelete = db.TaxiDrives.FirstOrDefault(td => td.TaxiDriveID.Equals(entityToDeleteID));
                        db.TaxiDrives.Remove(entityToDelete);

                        db.SaveChanges();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }

            return result;
        }

        public override TaxiDrive GetSingleEntityByKey(string key)
        {
            TaxiDrive result = null;

            using (TaxiDbContext db = new TaxiDbContext())
            {
                if (db.TaxiDrives.Any(td => td.TaxiDriveID.Equals(key)))
                {
                    try
                    {
                        result = db.TaxiDrives.Include(td => td.TaxiDriveDriver)
                                              .Include(td => td.TaxiDriveCustomer)
                                              .Include(td => td.TaxiDriveDispatcher)
                                              .Include(td => td.TaxiDriveComment)
                                              .Include(td => td.TaxiDriveStartingLocation)
                                              .Include(td => td.TaxiDriveDestination)
                                              .FirstOrDefault(td => td.TaxiDriveID.Equals(key));
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }

            return result;
        }

        public override IEnumerable<TaxiDrive> GetAll()
        {
            List<TaxiDrive> result = new List<TaxiDrive>();

            using (TaxiDbContext db = new TaxiDbContext())
            {
                try
                {
                    if (db.TaxiDrives.Count() > 0)
                    {
                        result = new List<TaxiDrive>(db.TaxiDrives.Include(td => td.TaxiDriveDriver)
                                                                  .Include(td => td.TaxiDriveCustomer)
                                                                  .Include(td => td.TaxiDriveDispatcher)
                                                                  .Include(td => td.TaxiDriveComment)
                                                                  .Include(td => td.TaxiDriveStartingLocation)
                                                                  .Include(td => td.TaxiDriveDestination)
                                                                  .ToList());
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return result;
        }

        public override bool Exists(string key)
        {
            bool result = false;

            using (TaxiDbContext db = new TaxiDbContext())
            {
                if (db.TaxiDrives.Any(td => td.TaxiDriveID.Equals(key)))
                {
                    result = true;
                }
            }

            return result;
        }
    }
}