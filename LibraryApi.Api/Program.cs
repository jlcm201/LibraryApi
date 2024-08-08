using LibraryApi.Api.Data;
using LibraryApi.Api.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;
using System;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Drawing;
using System.Diagnostics.Metrics;
using System.Net;
using LibraryApi.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseInMemoryDatabase("LibraryDB"));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ILibraryLoanService, LibraryLoanService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin() // Permitir cualquier origen
                   .AllowAnyHeader() // Permitir cualquier encabezado
                   .AllowAnyMethod(); // Permitir cualquier método
        });
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

SeedData(app);

app.UseCors("AllowAllOrigins");

app.Run();

void SeedData(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();

    context.Database.EnsureCreated();

    if (context.Books.Any()) return;

    // Categories
    var categoryFantasy = new Category { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Fantasía", Description = "La fantasía es un género literario que se caracteriza por la creación de mundos imaginarios y la inclusión de elementos sobrenaturales, mágicos o míticos. Los libros de fantasía suelen presentar criaturas mágicas, hechizos, y realidades alternativas que desafían las leyes de la naturaleza. Estos mundos ficticios permiten explorar temas y aventuras que no se encuentran en la realidad cotidiana. " };
    var categoryFiction = new Category { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Ficción Contemporánea", Description = "La ficción contemporánea abarca historias y temas relevantes para la vida moderna, generalmente situadas en el presente o en tiempos recientes. Los libros de este género exploran las complejidades de la vida actual y las experiencias personales de los personajes en contextos realistas. Pueden abordar cuestiones sociales, emocionales o filosóficas a través de tramas centradas en personajes detallados y situaciones realistas." };
    var categoryNovel = new Category { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Novela Clásica", Description = "La novela clásica se refiere a obras literarias que han sido reconocidas por su valor artístico y su influencia en la literatura. Estas novelas han resistido la prueba del tiempo y suelen ser estudiadas y admiradas por su estilo, estructura y profundidad temática. A menudo reflejan la cultura y las preocupaciones sociales de su época, pero también ofrecen insights universales que siguen siendo relevantes." };
    var categoryThriller = new Category { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Thriller/Misterio", Description = "El thriller y el misterio son géneros centrados en la creación de tensión, suspenso y enigma. Las novelas de thriller suelen tener una trama rápida y emocionante con giros inesperados y una constante sensación de peligro o urgencia. Los libros de misterio se enfocan en la resolución de un enigma, generalmente un crimen o asesinato, mediante la investigación de pistas y la deducción. Estos géneros buscan mantener al lector en vilo hasta la resolución final del conflicto." };
    var categoryHistorical = new Category { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Histórica", Description = "La novela histórica se centra en eventos, personajes y contextos del pasado. Estos libros utilizan escenarios históricos y hechos reales como telones de fondo para sus tramas, entrelazando personajes ficticios con la realidad histórica. El propósito es ofrecer una representación detallada y precisa de épocas anteriores, a menudo explorando temas como las costumbres, las políticas y las vidas de personas en tiempos históricos." };

    context.Categories.AddRange(categoryFantasy, categoryFiction, categoryNovel, categoryThriller, categoryHistorical);

    // Authors
    var authorJRRTolkien = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "J.R.R. Tolkien", Description = "John Ronald Reuel Tolkien (1892-1973) fue un escritor y filólogo británico, conocido como el padre de la literatura fantástica moderna. Su obra más célebre, \"El señor de los anillos\", junto con \"El hobbit\", ha tenido un impacto duradero en el género de la fantasía. Tolkien creó el extenso mundo de la Tierra Media y su obra se caracteriza por su profundidad lingüística y mitológica." };
    var authorJKRowling = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "J.K. Rowling", Description = "Joanne Rowling (1965-) es una autora británica que alcanzó la fama mundial con la serie \"Harry Potter\". La serie, que comienza con \"Harry Potter y la piedra filosofal\", sigue las aventuras de un joven mago en el colegio Hogwarts. Rowling ha sido reconocida por su habilidad para crear un mundo mágico y ha tenido un gran impacto en la literatura infantil y juvenil." };
    var authorPatrickRothfuss = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Patrick Rothfuss", Description = "Patrick Rothfuss (1973-) es un escritor estadounidense conocido por su serie de fantasía \"Crónica del asesino de reyes\". Su primera novela, \"El nombre del viento\", ha sido aclamada por su narrativa lírica y su construcción detallada del mundo. Rothfuss ha ganado un gran seguimiento de lectores con sus complejas tramas y personajes." };
    var authorGeorgeRRMartin = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "George R.R. Martin", Description = "George Raymond Richard Martin (1948-) es un autor y guionista estadounidense conocido por su serie de fantasía épica \"Canción de hielo y fuego\", que comienza con \"Juego de tronos\". Martin es famoso por su estilo detallado y sus intrincadas tramas políticas y sociales, así como por su enfoque en la moralidad ambigua de sus personajes." };
    var authorGabrielGarciaMarquez = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Gabriel García Márquez", Description = "Gabriel García Márquez (1927-2014) fue un escritor colombiano y uno de los principales exponentes del realismo mágico. Su obra más famosa, \"Cien años de soledad\", es una narración épica de la historia de la familia Buendía en el pueblo ficticio de Macondo. García Márquez recibió el Premio Nobel de Literatura en 1982 por su contribución a la literatura." };
    var authorHarperLee = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Harper Lee", Description = "Nelle Harper Lee (1926-2016) fue una escritora estadounidense conocida por su novela \"Matar a un ruiseñor\", que aborda temas de racismo y justicia en el sur de Estados Unidos durante la Gran Depresión. Su única novela publicada en vida ha tenido un impacto duradero en la literatura estadounidense y en el discurso sobre los derechos civiles." };
    var authorCarlosRuizZafon = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Carlos Ruiz Zafón", Description = "Carlos Ruiz Zafón (1964-2020) fue un autor español conocido por su serie \"El Cementerio de los Libros Olvidados\", que comienza con \"La sombra del viento\". Sus novelas combinan elementos de misterio, literatura y romance, y han sido traducidas a numerosos idiomas, ganando popularidad internacional." };
    var authorPauloCoelho = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Paulo Coelho", Description = "Paulo Coelho (1947-) es un autor brasileño cuyas obras suelen explorar temas espirituales y filosóficos. Su novela más famosa, \"El alquimista\", ha sido traducida a numerosos idiomas y ha tenido un gran impacto en la literatura de autoayuda y espiritualidad." };
    var authorStiegLarsson = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Stieg Larsson", Description = "Stieg Larsson (1954-2004) fue un escritor y periodista sueco, conocido principalmente por su trilogía \"Millennium\", que comienza con \"Los hombres que no amaban a las mujeres\". Larsson es famoso por sus tramas intensas de suspense y misterio, y sus novelas han sido adaptadas al cine y la televisión." };
    var authorMigueldeCervantes = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Miguel de Cervantes", Description = "Miguel de Cervantes (1547-1616) fue un escritor español, considerado uno de los más grandes novelistas de la literatura mundial. Su obra maestra, \"Don Quijote de la Mancha\", es una sátira de las novelas de caballería y un estudio profundo del idealismo y la realidad." };
    var authorJaneAusten = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Jane Austen", Description = "Jane Austen (1775-1817) fue una novelista británica conocida por sus obras que critican las costumbres sociales de su tiempo a través de la comedia romántica. \"Orgullo y prejuicio\", junto con otras novelas como \"Sentido y sensibilidad\", sigue siendo influyente en la literatura clásica." };
    var authorFScottFitzgerald = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "F. Scott Fitzgerald", Description = "Francis Scott Key Fitzgerald (1896-1940) fue un escritor estadounidense conocido por su novela \"El gran Gatsby\", que retrata la decadencia y el sueño americano en la década de 1920. Su estilo lírico y su crítica social han hecho de él una figura central en la literatura estadounidense del siglo XX." };
    var authorEmilyBronte = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Emily Brontë", Description = "Emily Brontë (1818-1848) fue una escritora británica cuya única novela, \"Cumbres borrascosas\", es un clásico de la literatura gótica. Su obra explora temas de amor, venganza y la naturaleza humana en un entorno oscuro y tormentoso." };
    var authorMarcelProust = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Marcel Proust", Description = "Marcel Proust (1871-1922) fue un novelista francés conocido por su monumental obra \"En busca del tiempo perdido\". La novela explora la memoria y el tiempo a través de una narrativa introspectiva y detallada, y ha tenido un profundo impacto en la literatura moderna." };
    var authorDanBrown = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Dan Brown", Description = "Dan Brown (1964-) es un autor estadounidense conocido por sus thrillers que combinan historia, arte y misterio. Su novela \"El código Da Vinci\" ha sido un éxito internacional, conocido por sus tramas intrigantes y su estilo de ritmo rápido." };
    var authorDaphneduMaurier = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Daphne du Maurier", Description = "Daphne du Maurier (1907-1989) fue una escritora británica conocida por sus novelas de misterio y suspense, como \"Rebeca\". Su obra a menudo explora temas de identidad y obsesión en contextos góticos y atmosféricos." };
    var authorPaulaHawkins = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Paula Hawkins", Description = "Paula Hawkins (1972-) es una autora británica que ganó fama con su novela \"La chica del tren\", un thriller psicológico que explora el impacto de los secretos y la percepción en la vida de una mujer." };
    var authorAgathaChristie = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Agatha Christie", Description = "Agatha Christie (1890-1976) fue una escritora británica, considerada la reina del crimen. Sus novelas de misterio, incluyendo \"Asesinato en el Orient Express\", presentan intrincados enredos y soluciones astutas, y han dejado una marca indeleble en el género del misterio." };
    var authorKenFollett = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Ken Follett", Description = "Ken Follett (1949-) es un autor británico conocido por sus novelas históricas y de suspense. \"Los pilares de la Tierra\", que trata sobre la construcción de una catedral en la Edad Media, es uno de sus trabajos más reconocidos." };
    var authorNoahGordon = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Noah Gordon", Description = "Noah Gordon (1926-2021) fue un autor estadounidense que escribió novelas históricas y contemporáneas. Su obra \"El médico\", sobre un joven que viaja a Persia para estudiar medicina, ha sido aclamada por su investigación y detalle histórico." };
    var authorIldefonsoFalcones = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Ildefonso Falcones", Description = "Ildefonso Falcones (1959-) es un autor español conocido por sus novelas históricas. \"La catedral del mar\", ambientada en la Barcelona medieval, es su obra más destacada, conocida por su detallada ambientación y narrativa épica." };
    var authorMarkusZusak = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Markus Zusak", Description = "Markus Zusak (1975-) es un autor australiano conocido por su novela \"La ladrona de libros\". La novela, ambientada en la Alemania nazi, ha sido aclamada por su estilo innovador y su tratamiento sensible de temas históricos y humanos." };
    var authorMiguelDelibes = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Miguel Delibes", Description = "Miguel Delibes (1920-2010) fue un novelista español conocido por sus obras que exploran la vida en el campo y las tensiones sociales en España. \"El hereje\", que aborda la persecución religiosa en el siglo XVI, es uno de sus trabajos más notables." };



    context.Authors.AddRange(authorJRRTolkien, authorJKRowling, authorPatrickRothfuss,
        authorGeorgeRRMartin, authorGabrielGarciaMarquez, authorHarperLee,
        authorCarlosRuizZafon, authorPauloCoelho, authorStiegLarsson,
        authorMigueldeCervantes, authorJaneAusten, authorFScottFitzgerald,
        authorEmilyBronte, authorMarcelProust, authorDanBrown,
        authorDaphneduMaurier, authorPaulaHawkins, authorAgathaChristie,
        authorKenFollett, authorNoahGordon, authorIldefonsoFalcones,
        authorMarkusZusak, authorMiguelDelibes);

    // Books
    var bookLordOfTheRings = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "El señor de los anillos", 
        Description = "Una épica trilogía que sigue la misión de un grupo de héroes para destruir un anillo maligno y salvar la Tierra Media de la oscuridad.", 
        AuthorId = authorJRRTolkien.Id, 
        CategoryId = categoryFantasy.Id,
        Copies = 2
     };
    var bookHarryPotter1 = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "Harry Potter y la piedra filosofal",
        Description = "El primer libro de la serie sobre un joven mago, Harry Potter, quien descubre su identidad mágica y asiste a Hogwarts, donde enfrenta desafíos y enemigos oscuros.",
        AuthorId = authorJKRowling.Id,
        CategoryId = categoryFantasy.Id,
        Copies = 2
	};
	var bookNombreDelViento = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "El nombre del viento",
        Description = "La historia de Kvothe, un joven prodigio que narra su vida desde su infancia hasta convertirse en un legendario mago y héroe.",
        AuthorId = authorPatrickRothfuss.Id,
        CategoryId = categoryFantasy.Id,
        Copies = 2
	};
	var bookAsesinoDeReyes = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "Crónica del asesino de reyes",
        Description = "El mismo universo de \"El nombre del viento\", continuando la saga de Kvothe con más aventuras y misterios en su búsqueda de la verdad sobre su pasado.",
        AuthorId = authorPatrickRothfuss.Id,
        CategoryId = categoryFantasy.Id,
        Copies = 2
	};
	var bookGameOfThrones = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "Juego de tronos",
        Description = "El primer libro de la serie \"Canción de hielo y fuego\", que presenta intrigas políticas, guerra y fantasía en el vasto continente de Westeros.",
        AuthorId = authorGeorgeRRMartin.Id,
        CategoryId = categoryFantasy.Id,
        Copies = 2
	};
	var bookCienAnyiosDeSoledad = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "Cien años de soledad",
        Description = "Una saga familiar que narra la vida de la familia Buendía a lo largo de varias generaciones en el pueblo ficticio de Macondo, explorando temas de amor, soledad y destino.",
        AuthorId = authorGabrielGarciaMarquez.Id,
        CategoryId = categoryFiction.Id,
        Copies = 2
	};
	var bookMatarUnRuisenyor = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "Matar a un ruiseñor",
        Description = "Ambientada en el sur de los EE.UU., cuenta la historia de Scout Finch y su padre, Atticus Finch, quien defiende a un hombre negro acusado injustamente de violación.",
        AuthorId = authorHarperLee.Id,
        CategoryId = categoryFiction.Id,
        Copies = 2
	};
	var bookSombraDelViento = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "La sombra del viento",
        Description = "Un joven descubre un libro en el Cementerio de los Libros Olvidados y se enreda en un misterio literario que afecta su vida y revela oscuros secretos.",
        AuthorId = authorCarlosRuizZafon.Id,
        CategoryId = categoryFiction.Id,
        Copies = 2
	};
	var bookElAlquimista = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "El alquimista",
        Description = "La historia de Santiago, un joven pastor que sigue sus sueños y busca un tesoro en Egipto, aprendiendo lecciones sobre la vida y el destino en el proceso.",
        AuthorId = authorPauloCoelho.Id,
        CategoryId = categoryFiction.Id,
        Copies = 2
	};
	var bookHombresQueNoAmaban = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "Los hombres que no amaban a las mujeres",
        Description = "Un periodista y una hacker investigan la desaparición de una joven hace 40 años, desentrañando oscuros secretos familiares y corrupción.",
        AuthorId = authorStiegLarsson.Id,
        CategoryId = categoryFiction.Id,
        Copies = 2
	};
	var bookDonQuijote = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "Don Quijote de la Mancha",
        Description = "La historia de un caballero que pierde la cordura y decide convertirse en un caballero andante, embarcándose en aventuras absurdas y enfrentando molinos de viento que confunde con gigantes.",
        AuthorId = authorMigueldeCervantes.Id,
        CategoryId = categoryNovel.Id,
        Copies = 2
	};
	var bookOrgulloYPrejuicio = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "Orgullo y prejuicio",
        Description = "Un clásico de la literatura que explora las relaciones sociales y los malentendidos románticos a través de la vida de Elizabeth Bennet y el apuesto pero orgulloso Sr. Darcy.",
        AuthorId = authorJaneAusten.Id,
        CategoryId = categoryNovel.Id,
        Copies = 2
	};
	var bookGranGatsby = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "El gran Gatsby",
        Description = "Ambientada en los años 20, sigue la vida de Jay Gatsby, un millonario en busca del amor de su vida y la decadencia de su mundo.",
        AuthorId = authorFScottFitzgerald.Id,
        CategoryId = categoryNovel.Id,
        Copies = 2
	};
	var bookCumbresBorrascosas = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "Cumbres borrascosas",
        Description = "Un intenso drama sobre amor y venganza en los páramos ingleses, centrado en la relación tumultuosa entre Heathcliff y Catherine Earnshaw.",
        AuthorId = authorEmilyBronte.Id,
        CategoryId = categoryNovel.Id,
        Copies = 2
	};
	var bookBuscaDelTiempo = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "En busca del tiempo perdido",
        Description = "Una extensa novela que explora la memoria y el tiempo a través de la vida del narrador, Marcel, y su entorno social en la Francia de finales del siglo XIX.",
        AuthorId = authorMarcelProust.Id,
        CategoryId = categoryNovel.Id,
        Copies = 2
	};
	var bookCodigoDaVinci = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "El código Da Vinci",
        Description = "Un thriller en el que un profesor de simbología y una criptóloga intentan desentrañar un misterio oculto en obras de arte y documentos históricos relacionados con el Santo Grial.",
        AuthorId = authorDanBrown.Id,
        CategoryId = categoryThriller.Id,
        Copies = 2
	};
	var bookRebeca = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "Rebeca",
        Description = "La historia de una joven que se convierte en la esposa del misterioso Max de Winter y se enfrenta a la sombra de su primera esposa, Rebecca, cuyo legado oscurece su vida.",
        AuthorId = authorDaphneduMaurier.Id,
        CategoryId = categoryThriller.Id,
        Copies = 2
	};
	var bookChicaDelTren = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "La chica del tren",
        Description = "Una novela de suspense que sigue a una mujer que, al observar una pareja desde el tren, se ve envuelta en un misterio tras la desaparición de la mujer que solía ver.",
        AuthorId = authorPaulaHawkins.Id,
        CategoryId = categoryThriller.Id,
        Copies = 2
	};
	var bookAsesinatoOrientExpress = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "Asesinato en el Orient Express",
        Description = "El famoso detective Hercule Poirot investiga un asesinato cometido a bordo del Orient Express, enfrentándose a un complicado rompecabezas de sospechosos.",
        AuthorId = authorAgathaChristie.Id,
        CategoryId = categoryThriller.Id,
        Copies = 2
	};
	var bookPilaresDeLaTierra = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "Los pilares de la Tierra",
        Description = "Una novela épica sobre la construcción de una catedral en la Inglaterra del siglo XII, llena de intrigas, luchas de poder y pasiones.",
        AuthorId = authorKenFollett.Id,
        CategoryId = categoryHistorical.Id,
        Copies = 2
	};
	var bookElMedico = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "El médico",
        Description = "La vida de Rob Cole, un joven huérfano en la Inglaterra medieval que viaja a Persia para estudiar medicina y descubrir su vocación.",
        AuthorId = authorNoahGordon.Id,
        CategoryId = categoryHistorical.Id,
        Copies = 2
	};
	var bookCatedralDelMar = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "La catedral del mar",
        Description = "Ambientada en la Barcelona del siglo XIV, sigue la vida de Arnau Estanyol mientras construye una catedral y enfrenta adversidades en su camino hacia la justicia y el éxito.",
        AuthorId = authorIldefonsoFalcones.Id,
        CategoryId = categoryHistorical.Id,
        Copies = 2
	};
	var bookLadronaDeLibros = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "La ladrona de libros",
        Description = "Ambientada en la Alemania nazi, narra la vida de Liesel Meminger, una joven que encuentra consuelo en los libros mientras enfrenta la devastación de la guerra.",
        AuthorId = authorMarkusZusak.Id,
        CategoryId = categoryHistorical.Id,
        Copies = 2
	};
	var bookElHereje = new Book { 
        Id = new Guid(), 
        CreatedOn = DateTime.Now,  
        Title = "El hereje",
        Description = "La historia de un hombre en la España del siglo XVI que desafía las creencias religiosas de su tiempo y enfrenta las consecuencias de su herejía.",
        AuthorId = authorMiguelDelibes.Id,
        CategoryId = categoryHistorical.Id,
        Copies = 2
	};

    context.Books.AddRange(bookLordOfTheRings, bookHarryPotter1,
        bookNombreDelViento, bookAsesinoDeReyes,
        bookGameOfThrones, bookCienAnyiosDeSoledad,
        bookMatarUnRuisenyor, bookSombraDelViento,
        bookElAlquimista, bookHombresQueNoAmaban,
        bookDonQuijote, bookOrgulloYPrejuicio,
        bookGranGatsby, bookCumbresBorrascosas,
        bookBuscaDelTiempo, bookCodigoDaVinci,
        bookRebeca, bookChicaDelTren,
        bookAsesinatoOrientExpress, bookPilaresDeLaTierra,
        bookElMedico, bookCatedralDelMar,
        bookLadronaDeLibros, bookElHereje);

    var userJL = new User { Id = new Guid(), CreatedOn = DateTime.Now, Name = "José Luis", SecondName = "Cruz" };
    var userAL = new User { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Angélica", SecondName = "Lima" };

    context.Users.AddRange(userJL, userAL);

    var loan1 = new LibraryLoan
    {
        Id = new Guid(),
        LoanDate = DateTime.Now,
        BookId = bookLordOfTheRings.Id,
        UserId = userJL.Id
    };
    var loan2 = new LibraryLoan
    {
        Id = new Guid(),
        LoanDate = DateTime.Now,
        BookId = bookElAlquimista.Id,
        UserId = userJL.Id
    };
    var loan3 = new LibraryLoan
    {
        Id = new Guid(),
        LoanDate = DateTime.Now,
        BookId = bookDonQuijote.Id,
        UserId = userAL.Id
    };

    context.LibraryLoans.AddRange(loan1, loan2, loan3);

    // Guardar cambios
    context.SaveChanges();
}
