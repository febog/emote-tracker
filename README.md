# emote-tracker
Emote tracker website.

## Database migrations

I commonly reset and add EF Core [database migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=vs) as I develop the data model. For reference, here are the steps that I follow.

These commands are to be executed in Visual Studio's *Package Manger Console* with the main web application project selected. Note that I specify the `-Context` since this project has more than one `DBContext`.

### Drop database and reset migrations

Deletes all migrations and creates a new migration with all the model information. This is especially useful early in development when the model can change drastically.

1. Delete the database.

```
Drop-Database -Context EmoteTrackerContext
```

2. Delete the Migrations folder, `Data/TrackerMigrations`.

3. Create a fresh migration with all the model data.

```
Add-Migration InitialMigration -Context EmoteTrackerContext -OutputDir Data/TrackerMigrations
```

4. Create the database and apply the migration.

```
Update-Database -Context EmoteTrackerContext
```

### Add a migration

1. Crate a new migration with the model changes.

```
Add-Migration ChangeDescription -Context EmoteTrackerContext
```

2. Push the changes to the database.

```
Update-Database -Context EmoteTrackerContext
```

### Deploying migrations

To run the database migrations on the App Service using SSH:

1. Navigate to the deployed files.

```
cd /home/site/wwwroot
```

2. Run the migration bundle created by the [deployment workflow](https://learn.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app?toc=%2Faspnet%2Fcore%2Ftoc.json&bc=%2Faspnet%2Fcore%2Fbreadcrumb%2Ftoc.json&view=aspnetcore-8.0#4-generate-database-schema).

```
./migrate
```
