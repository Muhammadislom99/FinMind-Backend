using FinMind.Infrastructure.Data.Context;

namespace FinMind.Infrastructure.Data.Seeders;

public static class DefaultUserSeeder
{
    public static void SeedData(FinMindDbContext context)
    {
        if (context.Users.Any()) return;

        using var transaction = context.Database.BeginTransaction();
        try
        {
            var userId = Guid.Parse("75a323dd-9422-41ac-8546-3e29f5250440");
            var user = new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com",
                PhoneNumber = "+992929100398",
                Password = "123",
            };

            context.Users.Add(user);
            context.SaveChanges();

            var mainAccount = new Account
            {
                Name = "Cash",
                Type = AccountType.Cash,
                CheckingAccount = true,
                UserId = userId
            };
            context.Accounts.Add(mainAccount);
            context.SaveChanges();

            var systemAccounts =
                new List<(string Name, AccountType Type, CategoryType CategoryType, string Icon, string Color, Guid?
                    ParentCategoryId)>
                {
                    ("Продукты", AccountType.Expense, CategoryType.Expense, "🛒", "#f44336", null),
                    ("Транспорт", AccountType.Expense, CategoryType.Expense, "🚗", "#2196f3", null),
                    ("Жильё", AccountType.Expense, CategoryType.Expense, "🏠", "#9c27b0", null),
                    ("Зарплата", AccountType.Income, CategoryType.Income, "💼", "#4caf50", null),
                    ("Подарки", AccountType.Income, CategoryType.Income, "🎁", "#ff9800", null)
                };

            var accountToCategoryMap = new Dictionary<string, Category>();

            foreach (var (name, accType, catType, icon, color, parentCategoryId) in systemAccounts)
            {
                var account = new Account
                {
                    Name = name,
                    Type = accType,
                    UserId = userId
                };
                context.Accounts.Add(account);
                context.SaveChanges();

                var category = new Category
                {
                    AccountId = account.Id,
                    Type = catType,
                    Icon = icon,
                    Color = color,
                    IsSystem = true,
                    ParentCategoryId = parentCategoryId
                };
                context.Categories.Add(category);
                context.SaveChanges();

                accountToCategoryMap[name] = category;
            }

            var subCategories = new List<(string Name, string ParentName, string Icon, string Color)>
            {
                ("Овощи и фрукты", "Продукты", "🥦", "#8bc34a"),
                ("Мясо и рыба", "Продукты", "🍖", "#ff5722")
            };

            foreach (var (name, parentName, icon, color) in subCategories)
            {
                if (!accountToCategoryMap.TryGetValue(parentName, out var parentCategory))
                    continue;

                var subAccount = new Account
                {
                    Name = name,
                    Type = AccountType.Expense,
                    UserId = userId
                };
                context.Accounts.Add(subAccount);
                context.SaveChanges();

                var subCategory = new Category
                {
                    AccountId = subAccount.Id,
                    Type = CategoryType.Expense,
                    Icon = icon,
                    Color = color,
                    IsSystem = true,
                    ParentCategoryId = parentCategory.Id
                };
                context.Categories.Add(subCategory);
                context.SaveChanges();
            }

            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }
}