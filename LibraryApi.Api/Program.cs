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

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ILibraryLoanService, LibraryLoanService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();

void SeedData(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<LibraryContext>();

    context.Database.EnsureCreated();

    if (context.Books.Any()) return;

    // Categories
    var categoryFantasy = new Category { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Fantas�a", Description = "La fantas�a es un g�nero literario que se caracteriza por la creaci�n de mundos imaginarios y la inclusi�n de elementos sobrenaturales, m�gicos o m�ticos. Los libros de fantas�a suelen presentar criaturas m�gicas, hechizos, y realidades alternativas que desaf�an las leyes de la naturaleza. Estos mundos ficticios permiten explorar temas y aventuras que no se encuentran en la realidad cotidiana. " };
    var categoryFiction = new Category { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Ficci�n Contempor�nea", Description = "La ficci�n contempor�nea abarca historias y temas relevantes para la vida moderna, generalmente situadas en el presente o en tiempos recientes. Los libros de este g�nero exploran las complejidades de la vida actual y las experiencias personales de los personajes en contextos realistas. Pueden abordar cuestiones sociales, emocionales o filos�ficas a trav�s de tramas centradas en personajes detallados y situaciones realistas." };
    var categoryNovel = new Category { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Novela Cl�sica", Description = "La novela cl�sica se refiere a obras literarias que han sido reconocidas por su valor art�stico y su influencia en la literatura. Estas novelas han resistido la prueba del tiempo y suelen ser estudiadas y admiradas por su estilo, estructura y profundidad tem�tica. A menudo reflejan la cultura y las preocupaciones sociales de su �poca, pero tambi�n ofrecen insights universales que siguen siendo relevantes." };
    var categoryThriller = new Category { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Thriller/Misterio", Description = "El thriller y el misterio son g�neros centrados en la creaci�n de tensi�n, suspenso y enigma. Las novelas de thriller suelen tener una trama r�pida y emocionante con giros inesperados y una constante sensaci�n de peligro o urgencia. Los libros de misterio se enfocan en la resoluci�n de un enigma, generalmente un crimen o asesinato, mediante la investigaci�n de pistas y la deducci�n. Estos g�neros buscan mantener al lector en vilo hasta la resoluci�n final del conflicto." };
    var categoryHistorical = new Category { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Hist�rica", Description = "La novela hist�rica se centra en eventos, personajes y contextos del pasado. Estos libros utilizan escenarios hist�ricos y hechos reales como telones de fondo para sus tramas, entrelazando personajes ficticios con la realidad hist�rica. El prop�sito es ofrecer una representaci�n detallada y precisa de �pocas anteriores, a menudo explorando temas como las costumbres, las pol�ticas y las vidas de personas en tiempos hist�ricos." };

    context.Categories.AddRange(categoryFantasy, categoryFiction, categoryNovel, categoryThriller, categoryHistorical);

    // Authors
    var authorJRRTolkien = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "J.R.R. Tolkien", Description = "John Ronald Reuel Tolkien (1892-1973) fue un escritor y fil�logo brit�nico, conocido como el padre de la literatura fant�stica moderna. Su obra m�s c�lebre, \"El se�or de los anillos\", junto con \"El hobbit\", ha tenido un impacto duradero en el g�nero de la fantas�a. Tolkien cre� el extenso mundo de la Tierra Media y su obra se caracteriza por su profundidad ling��stica y mitol�gica." };
    var authorJKRowling = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "J.K. Rowling", Description = "Joanne Rowling (1965-) es una autora brit�nica que alcanz� la fama mundial con la serie \"Harry Potter\". La serie, que comienza con \"Harry Potter y la piedra filosofal\", sigue las aventuras de un joven mago en el colegio Hogwarts. Rowling ha sido reconocida por su habilidad para crear un mundo m�gico y ha tenido un gran impacto en la literatura infantil y juvenil." };
    var authorPatrickRothfuss = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Patrick Rothfuss", Description = "Patrick Rothfuss (1973-) es un escritor estadounidense conocido por su serie de fantas�a \"Cr�nica del asesino de reyes\". Su primera novela, \"El nombre del viento\", ha sido aclamada por su narrativa l�rica y su construcci�n detallada del mundo. Rothfuss ha ganado un gran seguimiento de lectores con sus complejas tramas y personajes." };
    var authorGeorgeRRMartin = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "George R.R. Martin", Description = "George Raymond Richard Martin (1948-) es un autor y guionista estadounidense conocido por su serie de fantas�a �pica \"Canci�n de hielo y fuego\", que comienza con \"Juego de tronos\". Martin es famoso por su estilo detallado y sus intrincadas tramas pol�ticas y sociales, as� como por su enfoque en la moralidad ambigua de sus personajes." };
    var authorGabrielGarciaMarquez = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Gabriel Garc�a M�rquez", Description = "Gabriel Garc�a M�rquez (1927-2014) fue un escritor colombiano y uno de los principales exponentes del realismo m�gico. Su obra m�s famosa, \"Cien a�os de soledad\", es una narraci�n �pica de la historia de la familia Buend�a en el pueblo ficticio de Macondo. Garc�a M�rquez recibi� el Premio Nobel de Literatura en 1982 por su contribuci�n a la literatura." };
    var authorHarperLee = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Harper Lee", Description = "Nelle Harper Lee (1926-2016) fue una escritora estadounidense conocida por su novela \"Matar a un ruise�or\", que aborda temas de racismo y justicia en el sur de Estados Unidos durante la Gran Depresi�n. Su �nica novela publicada en vida ha tenido un impacto duradero en la literatura estadounidense y en el discurso sobre los derechos civiles." };
    var authorCarlosRuizZafon = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Carlos Ruiz Zaf�n", Description = "Carlos Ruiz Zaf�n (1964-2020) fue un autor espa�ol conocido por su serie \"El Cementerio de los Libros Olvidados\", que comienza con \"La sombra del viento\". Sus novelas combinan elementos de misterio, literatura y romance, y han sido traducidas a numerosos idiomas, ganando popularidad internacional." };
    var authorPauloCoelho = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Paulo Coelho", Description = "Paulo Coelho (1947-) es un autor brasile�o cuyas obras suelen explorar temas espirituales y filos�ficos. Su novela m�s famosa, \"El alquimista\", ha sido traducida a numerosos idiomas y ha tenido un gran impacto en la literatura de autoayuda y espiritualidad." };
    var authorStiegLarsson = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Stieg Larsson", Description = "Stieg Larsson (1954-2004) fue un escritor y periodista sueco, conocido principalmente por su trilog�a \"Millennium\", que comienza con \"Los hombres que no amaban a las mujeres\". Larsson es famoso por sus tramas intensas de suspense y misterio, y sus novelas han sido adaptadas al cine y la televisi�n." };
    var authorMigueldeCervantes = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Miguel de Cervantes", Description = "Miguel de Cervantes (1547-1616) fue un escritor espa�ol, considerado uno de los m�s grandes novelistas de la literatura mundial. Su obra maestra, \"Don Quijote de la Mancha\", es una s�tira de las novelas de caballer�a y un estudio profundo del idealismo y la realidad." };
    var authorJaneAusten = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Jane Austen", Description = "Jane Austen (1775-1817) fue una novelista brit�nica conocida por sus obras que critican las costumbres sociales de su tiempo a trav�s de la comedia rom�ntica. \"Orgullo y prejuicio\", junto con otras novelas como \"Sentido y sensibilidad\", sigue siendo influyente en la literatura cl�sica." };
    var authorFScottFitzgerald = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "F. Scott Fitzgerald", Description = "Francis Scott Key Fitzgerald (1896-1940) fue un escritor estadounidense conocido por su novela \"El gran Gatsby\", que retrata la decadencia y el sue�o americano en la d�cada de 1920. Su estilo l�rico y su cr�tica social han hecho de �l una figura central en la literatura estadounidense del siglo XX." };
    var authorEmilyBronte = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Emily Bront�", Description = "Emily Bront� (1818-1848) fue una escritora brit�nica cuya �nica novela, \"Cumbres borrascosas\", es un cl�sico de la literatura g�tica. Su obra explora temas de amor, venganza y la naturaleza humana en un entorno oscuro y tormentoso." };
    var authorMarcelProust = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Marcel Proust", Description = "Marcel Proust (1871-1922) fue un novelista franc�s conocido por su monumental obra \"En busca del tiempo perdido\". La novela explora la memoria y el tiempo a trav�s de una narrativa introspectiva y detallada, y ha tenido un profundo impacto en la literatura moderna." };
    var authorDanBrown = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Dan Brown", Description = "Dan Brown (1964-) es un autor estadounidense conocido por sus thrillers que combinan historia, arte y misterio. Su novela \"El c�digo Da Vinci\" ha sido un �xito internacional, conocido por sus tramas intrigantes y su estilo de ritmo r�pido." };
    var authorDaphneduMaurier = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Daphne du Maurier", Description = "Daphne du Maurier (1907-1989) fue una escritora brit�nica conocida por sus novelas de misterio y suspense, como \"Rebeca\". Su obra a menudo explora temas de identidad y obsesi�n en contextos g�ticos y atmosf�ricos." };
    var authorPaulaHawkins = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Paula Hawkins", Description = "Paula Hawkins (1972-) es una autora brit�nica que gan� fama con su novela \"La chica del tren\", un thriller psicol�gico que explora el impacto de los secretos y la percepci�n en la vida de una mujer." };
    var authorAgathaChristie = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Agatha Christie", Description = "Agatha Christie (1890-1976) fue una escritora brit�nica, considerada la reina del crimen. Sus novelas de misterio, incluyendo \"Asesinato en el Orient Express\", presentan intrincados enredos y soluciones astutas, y han dejado una marca indeleble en el g�nero del misterio." };
    var authorKenFollett = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Ken Follett", Description = "Ken Follett (1949-) es un autor brit�nico conocido por sus novelas hist�ricas y de suspense. \"Los pilares de la Tierra\", que trata sobre la construcci�n de una catedral en la Edad Media, es uno de sus trabajos m�s reconocidos." };
    var authorNoahGordon = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Noah Gordon", Description = "Noah Gordon (1926-2021) fue un autor estadounidense que escribi� novelas hist�ricas y contempor�neas. Su obra \"El m�dico\", sobre un joven que viaja a Persia para estudiar medicina, ha sido aclamada por su investigaci�n y detalle hist�rico." };
    var authorIldefonsoFalcones = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Ildefonso Falcones", Description = "Ildefonso Falcones (1959-) es un autor espa�ol conocido por sus novelas hist�ricas. \"La catedral del mar\", ambientada en la Barcelona medieval, es su obra m�s destacada, conocida por su detallada ambientaci�n y narrativa �pica." };
    var authorMarkusZusak = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Markus Zusak", Description = "Markus Zusak (1975-) es un autor australiano conocido por su novela \"La ladrona de libros\". La novela, ambientada en la Alemania nazi, ha sido aclamada por su estilo innovador y su tratamiento sensible de temas hist�ricos y humanos." };
    var authorMiguelDelibes = new Author { Id = new Guid(), CreatedOn = DateTime.Now, Name = "Miguel Delibes", Description = "Miguel Delibes (1920-2010) fue un novelista espa�ol conocido por sus obras que exploran la vida en el campo y las tensiones sociales en Espa�a. \"El hereje\", que aborda la persecuci�n religiosa en el siglo XVI, es uno de sus trabajos m�s notables." };

    context.Authors.AddRange(authorJRRTolkien, authorJKRowling, authorPatrickRothfuss,
        authorGeorgeRRMartin, authorGabrielGarciaMarquez, authorHarperLee,
        authorCarlosRuizZafon, authorPauloCoelho, authorStiegLarsson,
        authorMigueldeCervantes, authorJaneAusten, authorFScottFitzgerald,
        authorEmilyBronte, authorMarcelProust, authorDanBrown,
        authorDaphneduMaurier, authorPaulaHawkins, authorAgathaChristie,
        authorKenFollett, authorNoahGordon, authorIldefonsoFalcones,
        authorMarkusZusak, authorMiguelDelibes);

    // Books
    var bookLordOfTheRings = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "El señor de los anillos",
        Description = "Una épica trilog�a que sigue la misi�n de un grupo de h�roes para destruir un anillo maligno y salvar la Tierra Media de la oscuridad.",
        Copies = 2,
        AuthorId = authorJRRTolkien.Id,
        CategoryId = categoryFantasy.Id
    };
    var bookHarryPotter1 = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "Harry Potter y la piedra filosofal",
        Description = "El primer libro de la serie sobre un joven mago, Harry Potter, quien descubre su identidad m�gica y asiste a Hogwarts, donde enfrenta desaf�os y enemigos oscuros.",
        Copies = 2,
        AuthorId = authorJKRowling.Id,
        CategoryId = categoryFantasy.Id
    };
    var bookNombreDelViento = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "El nombre del viento",
        Description = "La historia de Kvothe, un joven prodigio que narra su vida desde su infancia hasta convertirse en un legendario mago y h�roe.",
        Copies = 2,
        AuthorId = authorPatrickRothfuss.Id,
        CategoryId = categoryFantasy.Id
    };
    var bookAsesinoDeReyes = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "Cr�nica del asesino de reyes",
        Description = "El mismo universo de \"El nombre del viento\", continuando la saga de Kvothe con m�s aventuras y misterios en su b�squeda de la verdad sobre su pasado.",
        Copies = 2,
        AuthorId = authorPatrickRothfuss.Id,
        CategoryId = categoryFantasy.Id
    };
    var bookGameOfThrones = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "Juego de tronos",
        Description = "El primer libro de la serie \"Canci�n de hielo y fuego\", que presenta intrigas pol�ticas, guerra y fantas�a en el vasto continente de Westeros.",
        Copies = 2,
        AuthorId = authorGeorgeRRMartin.Id,
        CategoryId = categoryFantasy.Id
    };
    var bookCienAnyiosDeSoledad = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "Cien a�os de soledad",
        Description = "Una saga familiar que narra la vida de la familia Buend�a a lo largo de varias generaciones en el pueblo ficticio de Macondo, explorando temas de amor, soledad y destino.",
        Copies = 2,
        AuthorId = authorGabrielGarciaMarquez.Id,
        CategoryId = categoryFiction.Id
    };
    var bookMatarUnRuisenyor = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "Matar a un ruise�or",
        Description = "Ambientada en el sur de los EE.UU., cuenta la historia de Scout Finch y su padre, Atticus Finch, quien defiende a un hombre negro acusado injustamente de violaci�n.",
        Copies = 2,
        AuthorId = authorHarperLee.Id,
        CategoryId = categoryFiction.Id
    };
    var bookSombraDelViento = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "La sombra del viento",
        Description = "Un joven descubre un libro en el Cementerio de los Libros Olvidados y se enreda en un misterio literario que afecta su vida y revela oscuros secretos.",
        Copies = 2,
        AuthorId = authorCarlosRuizZafon.Id,
        CategoryId = categoryFiction.Id
    };
    var bookElAlquimista = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "El alquimista",
        Description = "La historia de Santiago, un joven pastor que sigue sus sue�os y busca un tesoro en Egipto, aprendiendo lecciones sobre la vida y el destino en el proceso.",
        Copies = 2,
        AuthorId = authorPauloCoelho.Id,
        CategoryId = categoryFiction.Id
    };
    var bookHombresQueNoAmaban = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "Los hombres que no amaban a las mujeres",
        Description = "Un periodista y una hacker investigan la desaparici�n de una joven hace 40 a�os, desentra�ando oscuros secretos familiares y corrupci�n.",
        Copies = 2,
        AuthorId = authorStiegLarsson.Id,
        CategoryId = categoryFiction.Id
    };
    var bookDonQuijote = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "Don Quijote de la Mancha",
        Description = "La historia de un caballero que pierde la cordura y decide convertirse en un caballero andante, embarc�ndose en aventuras absurdas y enfrentando molinos de viento que confunde con gigantes.",
        Copies = 2,
        AuthorId = authorMigueldeCervantes.Id,
        CategoryId = categoryNovel.Id
    };
    var bookOrgulloYPrejuicio = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "Orgullo y prejuicio",
        Description = "Un cl�sico de la literatura que explora las relaciones sociales y los malentendidos rom�nticos a trav�s de la vida de Elizabeth Bennet y el apuesto pero orgulloso Sr. Darcy.",
        Copies = 2,
        AuthorId = authorJaneAusten.Id,
        CategoryId = categoryNovel.Id
    };
    var bookGranGatsby = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "El gran Gatsby",
        Description = "Ambientada en los a�os 20, sigue la vida de Jay Gatsby, un millonario en busca del amor de su vida y la decadencia de su mundo.",
        Copies = 2,
        AuthorId = authorFScottFitzgerald.Id,
        CategoryId = categoryNovel.Id
    };
    var bookCumbresBorrascosas = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "Cumbres borrascosas",
        Description = "Un intenso drama sobre amor y venganza en los p�ramos ingleses, centrado en la relaci�n tumultuosa entre Heathcliff y Catherine Earnshaw.",
        Copies = 2,
        AuthorId = authorEmilyBronte.Id,
        CategoryId = categoryNovel.Id
    };
    var bookBuscaDelTiempo = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "En busca del tiempo perdido",
        Description = "Una extensa novela que explora la memoria y el tiempo a trav�s de la vida del narrador, Marcel, y su entorno social en la Francia de finales del siglo XIX.",
        Copies = 2,
        AuthorId = authorMarcelProust.Id,
        CategoryId = categoryNovel.Id
    };
    var bookCodigoDaVinci = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "El c�digo Da Vinci",
        Description = "Un thriller en el que un profesor de simbolog�a y una cript�loga intentan desentra�ar un misterio oculto en obras de arte y documentos hist�ricos relacionados con el Santo Grial.",
        Copies = 2,
        AuthorId = authorDanBrown.Id,
        CategoryId = categoryThriller.Id
    };
    var bookRebeca = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "Rebeca",
        Description = "La historia de una joven que se convierte en la esposa del misterioso Max de Winter y se enfrenta a la sombra de su primera esposa, Rebecca, cuyo legado oscurece su vida.",
        Copies = 2,
        AuthorId = authorDaphneduMaurier.Id,
        CategoryId = categoryThriller.Id
    };
    var bookChicaDelTren = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "La chica del tren",
        Description = "Una novela de suspense que sigue a una mujer que, al observar una pareja desde el tren, se ve envuelta en un misterio tras la desaparici�n de la mujer que sol�a ver.",
        Copies = 2,
        AuthorId = authorPaulaHawkins.Id,
        CategoryId = categoryThriller.Id
    };
    var bookAsesinatoOrientExpress = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "Asesinato en el Orient Express",
        Description = "El famoso detective Hercule Poirot investiga un asesinato cometido a bordo del Orient Express, enfrent�ndose a un complicado rompecabezas de sospechosos.",
        Copies = 2,
        AuthorId = authorAgathaChristie.Id,
        CategoryId = categoryThriller.Id
    };
    var bookPilaresDeLaTierra = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "Los pilares de la Tierra",
        Description = "Una novela �pica sobre la construcci�n de una catedral en la Inglaterra del siglo XII, llena de intrigas, luchas de poder y pasiones.",
        Copies = 2,
        AuthorId = authorKenFollett.Id,
        CategoryId = categoryHistorical.Id
    };
    var bookElMedico = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "El m�dico",
        Description = "La vida de Rob Cole, un joven hu�rfano en la Inglaterra medieval que viaja a Persia para estudiar medicina y descubrir su vocaci�n.",
        Copies = 2,
        AuthorId = authorNoahGordon.Id,
        CategoryId = categoryHistorical.Id
    };
    var bookCatedralDelMar = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "La catedral del mar",
        Description = "Ambientada en la Barcelona del siglo XIV, sigue la vida de Arnau Estanyol mientras construye una catedral y enfrenta adversidades en su camino hacia la justicia y el �xito.",
        Copies = 2,
        AuthorId = authorIldefonsoFalcones.Id,
        CategoryId = categoryHistorical.Id
    };
    var bookLadronaDeLibros = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "La ladrona de libros",
        Description = "Ambientada en la Alemania nazi, narra la vida de Liesel Meminger, una joven que encuentra consuelo en los libros mientras enfrenta la devastaci�n de la guerra.",
        Copies = 2,
        AuthorId = authorMarkusZusak.Id,
        CategoryId = categoryHistorical.Id
    };
    var bookElHereje = new Book
    {
        Id = new Guid(),
        CreatedOn = DateTime.Now,
        Title = "El hereje",
        Description = "La historia de un hombre en la Espa�a del siglo XVI que desaf�a las creencias religiosas de su tiempo y enfrenta las consecuencias de su herej�a.",
        Copies = 2,
        AuthorId = authorMiguelDelibes.Id,
        CategoryId = categoryHistorical.Id
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
