using System;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using StudentApp.Models;

namespace StudentApp.Services;

public class StudentService
{

    private readonly IMongoCollection<Student> _students;

    public StudentService(IOptions<StudentDatabaseSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _students = database.GetCollection<Student>(settings.Value.StudentCollection);

            // Test MongoDB connection
            try
            {
                var result = database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Result;
                Console.WriteLine("✅ MongoDB connection established.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ MongoDB connection failed: {ex.Message}");
            }

    }

    // public async Task<List<Student>> GetAllAsync() => await _students.Find(_ => true).ToListAsync();

    public async Task CreateAsync(Student student) => await _students.InsertOneAsync(student);

    public async Task<Student?> GetByEmailAsync(string email) => await _students.Find(s => s.Email == email).FirstOrDefaultAsync();

    public async Task<List<Student>> GetAllAsync() => await _students.Find(_ => true).ToListAsync();

    public async Task<bool> DeleteByIdAsync(string id)
    {
        var result = await _students.DeleteOneAsync(s => s.Id == id);
        return result.DeletedCount > 0;
    }



}
