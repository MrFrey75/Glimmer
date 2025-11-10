using System.Reflection;
using Glimmer.Core.Enums;
using Glimmer.Core.Models;
using Glimmer.Core.Repositories;
using Glimmer.Core.Utils;
using Microsoft.Extensions.Logging;

namespace Glimmer.Core.Services;

public interface IDbSeedService
{
    Task SeedDatabaseAsync();
    Task<List<Species>> GetDefaultSpeciesAsync();
}

public class DbSeedService : IDbSeedService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<DbSeedService> _logger;

    public DbSeedService(IUserRepository userRepository, ILogger<DbSeedService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task SeedDatabaseAsync()
    {
        await EnsureSuperUserExistsAsync();
    }

    public async Task<List<Species>> GetDefaultSpeciesAsync()
    {
        var speciesList = new List<Species>();

        // Humanoid Species

        speciesList.Add(new Species
        {
            Name = "Human",
            Description = "Humans are the most widespread and adaptable species in the known universes. They exhibit a wide range of physical characteristics and cultural practices.",
            SpeciesType = SpeciesTypeEnum.Humanoid,
            AverageHeight = "5 to 6 feet",
            AverageLength = "N/A",
            AverageWeight = "100 to 250 pounds",
            EyeColor = "Varies (brown, blue, green, hazel)",
            HairColor = "Varies (black, brown, blonde, red)",
            SkinColor = "Varies (light to dark tones)",
            DistinguishingFeatures = "Diverse physical traits; no single distinguishing feature.",
            MagicalAbilitiesDescription = "Humans typically do not possess inherent magical abilities, but some individuals may develop magical skills through study or exposure."
        });

        speciesList.Add(new Species
        {
            Name = "Elf",
            Description = "Elves are a graceful and long-lived species known for their pointed ears and affinity for nature and magic. They often have a slender build and are taller than humans.",
            SpeciesType = SpeciesTypeEnum.Humanoid,
            AverageHeight = "6 to 7 feet",
            AverageLength = "N/A",
            AverageWeight = "120 to 180 pounds",
            EyeColor = "Commonly green, blue, or silver",
            HairColor = "Often blonde, silver, or black",
            SkinColor = "Typically fair, but can vary",
            DistinguishingFeatures = "Pointed ears, graceful features, and an ethereal appearance.",
            MagicalAbilitiesDescription = "Elves often possess innate magical abilities, particularly in areas related to nature, healing, and elemental manipulation."
        });

        speciesList.Add(new Species
        {
            Name = "Dwarf",
            Description = "Dwarves are a stout and hardy species known for their craftsmanship, mining skills, and strong connection to the earth. They typically have a stocky build and are shorter than humans.",
            SpeciesType = SpeciesTypeEnum.Humanoid,
            AverageHeight = "4 to 5 feet",
            AverageLength = "N/A",
            AverageWeight = "150 to 250 pounds",
            EyeColor = "Commonly brown, gray, or black",
            HairColor = "Often brown, black, or red",
            SkinColor = "Typically fair to tan",
            DistinguishingFeatures = "Stocky build, long beards (especially in males), and rugged features.",
            MagicalAbilitiesDescription = "Dwarves generally do not possess inherent magical abilities, but some may have skills in runic magic or earth-related enchantments."
        });

        // Common Domesticated Species

        speciesList.Add(new Species
        {
            Name = "Dog",
            Description = "Dogs are domesticated mammals known for their loyalty and companionship to humans. They come in various breeds with different sizes, shapes, and temperaments.",
            SpeciesType = SpeciesTypeEnum.Canine,
            AverageHeight = "1 to 3 feet (varies by breed)",
            AverageLength = "N/A",
            AverageWeight = "10 to 150 pounds (varies by breed)",
            EyeColor = "Varies (brown, blue, amber)",
            HairColor = "Varies (black, brown, white, mixed)",
            SkinColor = "Varies (pink, black, spotted)",
            DistinguishingFeatures = "Varied physical traits depending on breed; often have a keen sense of smell and hearing.",
            MagicalAbilitiesDescription = "Dogs do not possess magical abilities."
        });

        speciesList.Add(new Species
        {
            Name = "Cat",
            Description = "Cats are small, carnivorous mammals known for their agility, independence, and playful behavior. They are popular pets worldwide.",
            SpeciesType = SpeciesTypeEnum.Feline,
            AverageHeight = "9 to 10 inches at shoulder",
            AverageLength = "N/A",
            AverageWeight = "5 to 20 pounds",
            EyeColor = "Varies (green, yellow, blue)",
            HairColor = "Varies (black, white, orange, gray)",
            SkinColor = "Varies (pink, black)",
            DistinguishingFeatures = "Retractable claws, sharp vision, and a keen sense of hearing.",
            MagicalAbilitiesDescription = "Cats do not possess magical abilities."
        });
    // For all other species, add AverageLength = "N/A" if not present, or set AverageHeight = "N/A" and AverageLength = "<length>" if the species is best described by length (e.g., snakes, worms, etc.).
    // ...existing code for other species...
            
            // Common Farm Animals

            speciesList.Add(new Species
            {
                Name = "Horse",
                Description = "Horses are large, domesticated mammals used for riding, work, and companionship. They are known for their speed, strength, and grace.",
                SpeciesType = SpeciesTypeEnum.Equidae,
                AverageHeight = "4.5 to 6 feet at shoulder",
                AverageWeight = "800 to 2000 pounds",
                EyeColor = "Commonly brown or black",
                HairColor = "Varies (bay, chestnut, black, white, gray)",
                SkinColor = "Varies (pink, black, gray)",
                DistinguishingFeatures = "Long mane and tail, muscular build, and powerful legs.",
                MagicalAbilitiesDescription = "Horses do not possess magical abilities, though some mythical variants may have special powers."
            });

            speciesList.Add(new Species
            {
                Name = "Cow",
                Description = "Cows are large, domesticated mammals raised for milk, meat, and leather. They are gentle herbivores with a calm disposition.",
                SpeciesType = SpeciesTypeEnum.Ruminant,
                AverageHeight = "4 to 5 feet at shoulder",
                AverageWeight = "1000 to 1800 pounds",
                EyeColor = "Commonly brown or black",
                HairColor = "Varies (black, brown, white, spotted)",
                SkinColor = "Varies (pink, black)",
                DistinguishingFeatures = "Large udders (in females), horns (in some breeds), and distinctive mooing sounds.",
                MagicalAbilitiesDescription = "Cows do not possess magical abilities."
            });

            speciesList.Add(new Species
            {
                Name = "Pig",
                Description = "Pigs are intelligent, domesticated mammals raised for meat. They are omnivores known for their adaptability and social behavior.",
                SpeciesType = SpeciesTypeEnum.Swine,
                AverageHeight = "2 to 3 feet at shoulder",
                AverageWeight = "200 to 800 pounds",
                EyeColor = "Commonly brown or black",
                HairColor = "Varies (pink, black, brown, spotted)",
                SkinColor = "Typically pink or black",
                DistinguishingFeatures = "Snout for rooting, curly tail, and stocky build.",
                MagicalAbilitiesDescription = "Pigs do not possess magical abilities."
            });

            speciesList.Add(new Species
            {
                Name = "Sheep",
                Description = "Sheep are domesticated mammals raised for wool, meat, and milk. They are herbivores known for their flocking behavior and wooly coats.",
                SpeciesType = SpeciesTypeEnum.Ungulate,
                AverageHeight = "2 to 3 feet at shoulder",
                AverageWeight = "100 to 300 pounds",
                EyeColor = "Commonly brown or black",
                HairColor = "Typically white wool, but can vary",
                SkinColor = "Usually pink or black",
                DistinguishingFeatures = "Wooly fleece, rectangular pupils, and bleating sounds.",
                MagicalAbilitiesDescription = "Sheep do not possess magical abilities."
            });

            speciesList.Add(new Species
            {
                Name = "Goat",
                Description = "Goats are hardy, domesticated mammals known for their climbing ability and varied diet. They are raised for milk, meat, and fiber.",
                SpeciesType = SpeciesTypeEnum.Ruminant,
                AverageHeight = "2 to 3.5 feet at shoulder",
                AverageWeight = "60 to 300 pounds",
                EyeColor = "Commonly brown, yellow, or blue",
                HairColor = "Varies (white, black, brown, mixed)",
                SkinColor = "Varies (pink, black)",
                DistinguishingFeatures = "Horns, beards (especially in males), and rectangular pupils.",
                MagicalAbilitiesDescription = "Goats do not possess magical abilities."
            });

            speciesList.Add(new Species
            {
                Name = "Chicken",
                Description = "Chickens are domesticated birds raised for eggs and meat. They are social creatures that live in flocks and exhibit complex behaviors.",
                SpeciesType = SpeciesTypeEnum.Bird,
                AverageHeight = "1 to 2 feet",
                AverageWeight = "2 to 10 pounds",
                EyeColor = "Commonly orange, red, or brown",
                HairColor = "N/A (feathers vary: white, brown, black, red)",
                SkinColor = "Usually yellow or pink",
                DistinguishingFeatures = "Combs and wattles, ability to lay eggs, and distinctive clucking sounds.",
                MagicalAbilitiesDescription = "Chickens do not possess magical abilities."
            });

            speciesList.Add(new Species
            {
                Name = "Duck",
                Description = "Ducks are domesticated waterfowl raised for eggs, meat, and feathers. They are excellent swimmers and have waterproof feathers.",
                SpeciesType = SpeciesTypeEnum.Bird,
                AverageHeight = "1 to 2 feet",
                AverageWeight = "2 to 8 pounds",
                EyeColor = "Commonly brown or black",
                HairColor = "N/A (feathers vary: white, brown, green, black)",
                SkinColor = "Usually yellow or orange",
                DistinguishingFeatures = "Webbed feet, waterproof feathers, and distinctive quacking sounds.",
                MagicalAbilitiesDescription = "Ducks do not possess magical abilities."
            });

        // Common Forest Species

        speciesList.Add(new Species
        {
            Name = "Deer",
            Description = "Deer are graceful herbivorous mammals commonly found in forests and woodlands. They are known for their agility and keen senses.",
            SpeciesType = SpeciesTypeEnum.Mammal,
            AverageHeight = "3 to 4 feet at shoulder",
            AverageWeight = "100 to 300 pounds",
            EyeColor = "Commonly brown or black",
            HairColor = "Varies (brown, reddish-brown, gray)",
            SkinColor = "Usually brown or tan",
            DistinguishingFeatures = "Antlers (in males of most species), slender legs, and white-spotted coats in some species.",
            MagicalAbilitiesDescription = "Deer do not possess magical abilities, though some mythical variants may have special powers."
        });

        speciesList.Add(new Species
        {
            Name = "Wolf",
            Description = "Wolves are intelligent pack hunters and the ancestors of domestic dogs. They are social predators with strong family bonds.",
            SpeciesType = SpeciesTypeEnum.Canine,
            AverageHeight = "2.5 to 3 feet at shoulder",
            AverageWeight = "50 to 150 pounds",
            EyeColor = "Commonly yellow, amber, or brown",
            HairColor = "Varies (gray, black, white, brown)",
            SkinColor = "Usually black or gray",
            DistinguishingFeatures = "Powerful jaws, keen senses, and distinctive howling calls.",
            MagicalAbilitiesDescription = "Wolves do not possess magical abilities, though some mythical variants may have special powers."
        });

        speciesList.Add(new Species
        {
            Name = "Bear",
            Description = "Bears are large, powerful omnivorous mammals found in forests worldwide. They are known for their strength and adaptability.",
            SpeciesType = SpeciesTypeEnum.Mammal,
            AverageHeight = "3 to 5 feet at shoulder",
            AverageWeight = "200 to 800 pounds",
            EyeColor = "Commonly brown or black",
            HairColor = "Varies (brown, black, blonde, white)",
            SkinColor = "Usually black or brown",
            DistinguishingFeatures = "Massive build, powerful claws, and excellent sense of smell.",
            MagicalAbilitiesDescription = "Bears do not possess magical abilities."
        });

        speciesList.Add(new Species
        {
            Name = "Rabbit",
            Description = "Rabbits are small, fast herbivorous mammals known for their long ears and powerful hind legs. They are common prey animals in forest ecosystems.",
            SpeciesType = SpeciesTypeEnum.Mammal,
            AverageHeight = "8 to 12 inches",
            AverageWeight = "2 to 10 pounds",
            EyeColor = "Commonly brown or black",
            HairColor = "Varies (brown, gray, white, black)",
            SkinColor = "Usually pink or gray",
            DistinguishingFeatures = "Long ears, powerful hind legs, and fluffy tails.",
            MagicalAbilitiesDescription = "Rabbits do not possess magical abilities."
        });

        speciesList.Add(new Species
        {
            Name = "Squirrel",
            Description = "Squirrels are small, agile mammals known for their bushy tails and tree-climbing abilities. They are excellent at storing food for winter.",
            SpeciesType = SpeciesTypeEnum.Rodent,
            AverageHeight = "6 to 12 inches",
            AverageWeight = "1 to 2 pounds",
            EyeColor = "Commonly brown or black",
            HairColor = "Varies (gray, brown, red, black)",
            SkinColor = "Usually pink or gray",
            DistinguishingFeatures = "Bushy tail, sharp claws for climbing, and cheek pouches for storing food.",
            MagicalAbilitiesDescription = "Squirrels do not possess magical abilities."
        });

        speciesList.Add(new Species
        {
            Name = "Fox",
            Description = "Foxes are cunning and adaptable small canines known for their intelligence and beautiful coats. They are solitary hunters with excellent hearing.",
            SpeciesType = SpeciesTypeEnum.Canine,
            AverageHeight = "1.5 to 2 feet at shoulder",
            AverageWeight = "10 to 30 pounds",
            EyeColor = "Commonly amber, yellow, or brown",
            HairColor = "Varies (red, silver, gray, white)",
            SkinColor = "Usually black or gray",
            DistinguishingFeatures = "Pointed snout, bushy tail, and triangular ears.",
            MagicalAbilitiesDescription = "Foxes do not possess magical abilities, though some mythical variants may have special powers."
        });

        speciesList.Add(new Species
        {
            Name = "Owl",
            Description = "Owls are nocturnal birds of prey known for their silent flight and exceptional night vision. They are skilled hunters and often associated with wisdom.",
            SpeciesType = SpeciesTypeEnum.Bird,
            AverageHeight = "1 to 2.5 feet",
            AverageWeight = "1 to 10 pounds",
            EyeColor = "Commonly yellow, orange, or brown",
            HairColor = "N/A (feathers vary: brown, gray, white, black)",
            SkinColor = "Usually gray or yellow",
            DistinguishingFeatures = "Large eyes, silent flight, and distinctive hooting calls.",
            MagicalAbilitiesDescription = "Owls do not possess magical abilities, though they are often associated with wisdom and magic in folklore."
        });

        speciesList.Add(new Species
        {
            Name = "Raccoon",
            Description = "Raccoons are intelligent, nocturnal mammals known for their distinctive facial markings and dexterous front paws. They are excellent problem solvers.",
            SpeciesType = SpeciesTypeEnum.Mammal,
            AverageHeight = "1 to 1.5 feet at shoulder",
            AverageWeight = "15 to 40 pounds",
            EyeColor = "Commonly brown or black",
            HairColor = "Gray and black with distinctive facial markings",
            SkinColor = "Usually black or gray",
            DistinguishingFeatures = "Black mask around eyes, ringed tail, and highly dexterous front paws.",
            MagicalAbilitiesDescription = "Raccoons do not possess magical abilities."
        });



        // Common Desret Species

        speciesList.Add(new Species
        {
            Name = "Camel",
            Description = "Camels are large, desert-dwelling mammals known for their ability to survive in arid environments. They can go long periods without water and are excellent pack animals.",
            SpeciesType = SpeciesTypeEnum.Dromedary,
            AverageHeight = "6 to 7 feet at shoulder",
            AverageWeight = "900 to 1600 pounds",
            EyeColor = "Commonly brown or black",
            HairColor = "Varies (tan, brown, cream)",
            SkinColor = "Usually tan or brown",
            DistinguishingFeatures = "One or two humps on back, long legs, and thick eyelashes to protect from sand.",
            MagicalAbilitiesDescription = "Camels do not possess magical abilities."
        });

        speciesList.Add(new Species
        {
            Name = "Fennec Fox",
            Description = "Fennec foxes are small desert foxes known for their oversized ears and nocturnal lifestyle. They are well-adapted to extreme heat and arid conditions.",
            SpeciesType = SpeciesTypeEnum.Canine,
            AverageHeight = "8 to 10 inches at shoulder",
            AverageWeight = "2 to 3 pounds",
            EyeColor = "Commonly black or brown",
            HairColor = "Cream, sandy, or pale yellow",
            SkinColor = "Usually pink or gray",
            DistinguishingFeatures = "Extremely large ears, small size, and thick fur on feet to protect from hot sand.",
            MagicalAbilitiesDescription = "Fennec foxes do not possess magical abilities."
        });

        speciesList.Add(new Species
        {
            Name = "Desert Scorpion",
            Description = "Desert scorpions are arachnids well-adapted to harsh desert environments. They are nocturnal predators with venomous stingers.",
            SpeciesType = SpeciesTypeEnum.Arthropod,
            AverageHeight = "2 to 6 inches",
            AverageWeight = "0.5 to 2 ounces",
            EyeColor = "Black or dark brown",
            HairColor = "N/A (exoskeleton varies: tan, brown, black)",
            SkinColor = "N/A (chitinous exoskeleton)",
            DistinguishingFeatures = "Pincers, segmented tail with stinger, and ability to glow under UV light.",
            MagicalAbilitiesDescription = "Desert scorpions do not possess magical abilities."
        });

        speciesList.Add(new Species
        {
            Name = "Sidewinder Snake",
            Description = "Sidewinder snakes are venomous pit vipers that move in a distinctive sidewinding motion across desert sand. They are excellent ambush predators.",
            SpeciesType = SpeciesTypeEnum.Snake,
            AverageHeight = "N/A (length: 1.5 to 2.5 feet)",
            AverageWeight = "0.5 to 1 pound",
            EyeColor = "Commonly yellow or brown",
            HairColor = "N/A (scales vary: tan, brown, with darker patterns)",
            SkinColor = "N/A (scaled)",
            DistinguishingFeatures = "Horned scales above eyes, sidewinding locomotion, and heat-sensing pits.",
            MagicalAbilitiesDescription = "Sidewinder snakes do not possess magical abilities."
        });

        speciesList.Add(new Species
        {
            Name = "Desert Tortoise",
            Description = "Desert tortoises are slow-moving reptiles that can live for over 100 years. They are well-adapted to arid environments and can store water in their bodies.",
            SpeciesType = SpeciesTypeEnum.Turtle,
            AverageHeight = "6 to 14 inches",
            AverageWeight = "8 to 15 pounds",
            EyeColor = "Commonly brown or black",
            HairColor = "N/A (shell varies: brown, tan, with growth rings)",
            SkinColor = "Usually gray or brown",
            DistinguishingFeatures = "High-domed shell, stumpy legs, and ability to retract completely into shell.",
            MagicalAbilitiesDescription = "Desert tortoises do not possess magical abilities."
        });

        speciesList.Add(new Species
        {
            Name = "Roadrunner",
            Description = "Roadrunners are fast-running ground birds found in desert regions. They are excellent runners and can reach speeds up to 20 mph.",
            SpeciesType = SpeciesTypeEnum.Bird,
            AverageHeight = "1 to 2 feet",
            AverageWeight = "8 to 24 ounces",
            EyeColor = "Commonly brown or yellow",
            HairColor = "N/A (feathers: brown and white streaked)",
            SkinColor = "Usually blue or purple around eyes",
            DistinguishingFeatures = "Long legs for running, distinctive crest, and ability to fly short distances.",
            MagicalAbilitiesDescription = "Roadrunners do not possess magical abilities."
        });

        speciesList.Add(new Species
        {
            Name = "Coyote",
            Description = "Coyotes are adaptable canines found throughout desert regions. They are intelligent pack hunters and scavengers with distinctive howls.",
            SpeciesType = SpeciesTypeEnum.Canine,
            AverageHeight = "2 to 2.5 feet at shoulder",
            AverageWeight = "20 to 50 pounds",
            EyeColor = "Commonly yellow or amber",
            HairColor = "Varies (gray, tan, brown, reddish)",
            SkinColor = "Usually black or gray",
            DistinguishingFeatures = "Pointed ears, narrow snout, and distinctive yipping and howling calls.",
            MagicalAbilitiesDescription = "Coyotes do not possess magical abilities, though they feature prominently in Native American folklore and mythology."
        });

        // Lord of the Rings Species
        
        speciesList.Add(new Species
        {
            Name = "Hobbit",
            Description = "Hobbits are a diminutive and peace-loving humanoid species known for their love of comfort, good food, and simple pleasures. They typically live in underground homes called hobbit-holes.",
            SpeciesType = SpeciesTypeEnum.Humanoid,
            AverageHeight = "2 to 4 feet",
            AverageWeight = "40 to 80 pounds",
            EyeColor = "Commonly brown, blue, or green",
            HairColor = "Often brown or sandy blonde",
            SkinColor = "Fair to tan",
            DistinguishingFeatures = "Large, hairy feet; no need for shoes; pointed ears; exceptional stealth.",
            MagicalAbilitiesDescription = "Hobbits do not possess inherent magical abilities, though they show remarkable resistance to corruption and dark magic."
        });

        speciesList.Add(new Species
        {
            Name = "Orc",
            Description = "Orcs are a brutish and warlike humanoid species, often serving dark powers. They are known for their strength, ferocity, and hatred of sunlight.",
            SpeciesType = SpeciesTypeEnum.Humanoid,
            AverageHeight = "5 to 7 feet",
            AverageWeight = "180 to 300 pounds",
            EyeColor = "Commonly yellow, red, or black",
            HairColor = "Often black or dark brown",
            SkinColor = "Typically green, gray, or brown",
            DistinguishingFeatures = "Pointed ears, sharp teeth, hunched posture, and scarred skin.",
            MagicalAbilitiesDescription = "Orcs generally lack magical abilities but may possess enhanced strength and endurance."
        });

        speciesList.Add(new Species
        {
            Name = "Ent",
            Description = "Ents are ancient, tree-like beings who serve as shepherds of the forest. They are slow to anger but tremendously powerful when roused.",
            SpeciesType = SpeciesTypeEnum.Plant,
            AverageHeight = "14 to 20 feet",
            AverageWeight = "2000 to 5000 pounds",
            EyeColor = "Commonly green or brown",
            HairColor = "N/A (foliage varies with seasons)",
            SkinColor = "Bark-like, brown to gray",
            DistinguishingFeatures = "Tree-like appearance, extremely long lifespan, slow and deliberate movement.",
            MagicalAbilitiesDescription = "Ents possess nature magic, can communicate with trees, and have immense physical strength."
        });

        // Harry Potter Species

        speciesList.Add(new Species
        {
            Name = "House Elf",
            Description = "House elves are small, magical creatures bound to serve wizarding families. Despite their servitude, they possess powerful magic.",
            SpeciesType = SpeciesTypeEnum.Humanoid,
            AverageHeight = "2 to 3 feet",
            AverageWeight = "20 to 40 pounds",
            EyeColor = "Commonly large and bulbous, various colors",
            HairColor = "Sparse or none",
            SkinColor = "Varies (pale, brown, gray)",
            DistinguishingFeatures = "Large ears, bulbous eyes, thin frame, and bound to magical families through enchantment.",
            MagicalAbilitiesDescription = "House elves possess powerful innate magic including teleportation, object manipulation, and cleaning enchantments that can surpass wizard capabilities."
        });

        speciesList.Add(new Species
        {
            Name = "Phoenix",
            Description = "Phoenixes are magnificent, immortal birds that burst into flames upon death and are reborn from their own ashes. They are symbols of renewal and healing.",
            SpeciesType = SpeciesTypeEnum.Mythical,
            AverageHeight = "3 to 4 feet",
            AverageWeight = "20 to 30 pounds",
            EyeColor = "Commonly bright gold or crimson",
            HairColor = "N/A (plumage is scarlet and gold)",
            SkinColor = "N/A",
            DistinguishingFeatures = "Brilliant scarlet and gold plumage, tail feathers of immense magical power, tears with healing properties.",
            MagicalAbilitiesDescription = "Phoenixes possess rebirth through fire, healing tears, incredible strength for their size, and their tail feathers can be used as wand cores."
        });

        speciesList.Add(new Species
        {
            Name = "Hippogriff",
            Description = "Hippogriffs are proud magical creatures with the front half of a giant eagle and the back half of a horse. They demand respect and proper etiquette.",
            SpeciesType = SpeciesTypeEnum.Hybrid,
            AverageHeight = "9 feet at shoulder",
            AverageWeight = "1000 to 1500 pounds",
            EyeColor = "Commonly orange or gold",
            HairColor = "Varies (feathers on front, horse coat on rear)",
            SkinColor = "N/A",
            DistinguishingFeatures = "Eagle head and wings, horse body, extremely proud demeanor, requires a bow of respect.",
            MagicalAbilitiesDescription = "Hippogriffs can fly at great speeds and have enhanced senses. They are intelligent and understand complex emotions."
        });

        // Alien/Sci-Fi Species

        speciesList.Add(new Species
        {
            Name = "Vulcan",
            Description = "Vulcans are a humanoid species known for their logical minds, suppression of emotions, and adherence to reason and science.",
            SpeciesType = SpeciesTypeEnum.Alien,
            AverageHeight = "5.5 to 6.5 feet",
            AverageWeight = "120 to 200 pounds",
            EyeColor = "Commonly brown or black",
            HairColor = "Often black or dark brown",
            SkinColor = "Typically pale with greenish undertones",
            DistinguishingFeatures = "Pointed ears, arched eyebrows, copper-based blood (green), superior strength and telepathic abilities.",
            MagicalAbilitiesDescription = "Vulcans possess telepathic abilities, particularly the mind-meld, and have superior physical strength and longevity compared to humans."
        });

        speciesList.Add(new Species
        {
            Name = "Klingon",
            Description = "Klingons are a warrior species known for their honor-bound culture, physical prowess, and aggressive nature.",
            SpeciesType = SpeciesTypeEnum.Alien,
            AverageHeight = "6 to 7 feet",
            AverageWeight = "180 to 280 pounds",
            EyeColor = "Commonly brown or black",
            HairColor = "Often black or dark brown",
            SkinColor = "Typically dark brown with red or purple tones",
            DistinguishingFeatures = "Prominent cranial ridges, redundant organs, exceptional strength and endurance.",
            MagicalAbilitiesDescription = "Klingons do not possess magical abilities but have redundant biological systems, superior strength, and high pain tolerance."
        });

        speciesList.Add(new Species
        {
            Name = "Wookiee",
            Description = "Wookiees are tall, hairy bipeds from the planet Kashyyyk, known for their strength, loyalty, and mechanical aptitude.",
            SpeciesType = SpeciesTypeEnum.Alien,
            AverageHeight = "7 to 8 feet",
            AverageWeight = "200 to 350 pounds",
            EyeColor = "Commonly blue, brown, or yellow",
            HairColor = "Varies (brown, black, white, auburn)",
            SkinColor = "Hidden by fur",
            DistinguishingFeatures = "Covered in thick fur, exceptional strength, communicate through growls and roars (Shyriiwook language).",
            MagicalAbilitiesDescription = "Wookiees do not possess magical abilities but have extraordinary strength, climbing skills, and long lifespans (up to 400 years)."
        });

        speciesList.Add(new Species
        {
            Name = "Twi'lek",
            Description = "Twi'leks are a humanoid species characterized by their colorful skin and distinctive head-tails called lekku, which are used for communication.",
            SpeciesType = SpeciesTypeEnum.Alien,
            AverageHeight = "5 to 6 feet",
            AverageWeight = "100 to 180 pounds",
            EyeColor = "Varies widely",
            HairColor = "None (lekku instead)",
            SkinColor = "Varies greatly (blue, green, orange, red, pink, yellow)",
            DistinguishingFeatures = "Two head-tails (lekku) that contain part of their brain, colorful skin, no body hair.",
            MagicalAbilitiesDescription = "Twi'leks do not possess inherent magical abilities, though some can be Force-sensitive."
        });

        // Dune Species

        speciesList.Add(new Species
        {
            Name = "Sandworm (Shai-Hulud)",
            Description = "Sandworms are colossal, semi-sentient creatures native to Arrakis. They are the source of the spice melange and are revered by the Fremen as manifestations of God.",
            SpeciesType = SpeciesTypeEnum.Invertebrate,
            AverageHeight = "N/A (length: 1000 to 1500 feet)",
            AverageWeight = "Several million tons",
            EyeColor = "N/A",
            HairColor = "N/A",
            SkinColor = "Orange-brown with crystalline rings",
            DistinguishingFeatures = "Massive segmented body, crystalline teeth hundreds of feet long, produces spice melange, attracted to rhythmic vibrations.",
            MagicalAbilitiesDescription = "Sandworms are the source of the spice melange through their lifecycle. They are immune to most weapons and live for thousands of years."
        });

        speciesList.Add(new Species
        {
            Name = "Fremen",
            Description = "Fremen are human inhabitants of Arrakis who have adapted to the harsh desert environment through cultural practices and physiological changes.",
            SpeciesType = SpeciesTypeEnum.AugmentedHumanoid,
            AverageHeight = "5 to 6 feet",
            AverageWeight = "100 to 180 pounds",
            EyeColor = "Blue-within-blue (spice saturation)",
            HairColor = "Typically dark",
            SkinColor = "Tan to dark from sun exposure",
            DistinguishingFeatures = "Blue-within-blue eyes from spice exposure, exceptional desert survival skills, water discipline.",
            MagicalAbilitiesDescription = "Extended spice exposure grants Fremen limited prescient abilities and enhanced physical capabilities. Some develop stronger psychic powers."
        });

        // Dungeons & Dragons Species

        speciesList.Add(new Species
        {
            Name = "Tiefling",
            Description = "Tieflings are humanoids with demonic or infernal ancestry, bearing physical marks of their heritage such as horns, tails, and unusual skin colors.",
            SpeciesType = SpeciesTypeEnum.Demon,
            AverageHeight = "5 to 6.5 feet",
            AverageWeight = "120 to 200 pounds",
            EyeColor = "Often solid colors without visible sclera (red, gold, silver)",
            HairColor = "Varies (often dark, sometimes with unusual colors)",
            SkinColor = "Human range plus red, blue, purple, or other infernal hues",
            DistinguishingFeatures = "Horns, tail, unusual skin color, sharp teeth, and sometimes hooves or other fiendish traits.",
            MagicalAbilitiesDescription = "Tieflings have innate resistance to fire and can cast minor illusion and hellish rebuke spells naturally."
        });

        speciesList.Add(new Species
        {
            Name = "Dragonborn",
            Description = "Dragonborn are proud, honorable humanoids with draconic ancestry, featuring scales, claws, and the ability to breathe elemental energy.",
            SpeciesType = SpeciesTypeEnum.Dragon,
            AverageHeight = "6 to 7 feet",
            AverageWeight = "200 to 300 pounds",
            EyeColor = "Varies by draconic ancestry",
            HairColor = "None (scales instead)",
            SkinColor = "Scaled skin in various colors (red, gold, blue, green, black, white, etc.)",
            DistinguishingFeatures = "Draconic features including scales, claws, fangs, and lack of hair or tail.",
            MagicalAbilitiesDescription = "Dragonborn can breathe elemental energy (fire, lightning, acid, cold, or poison) based on their draconic ancestry."
        });

        speciesList.Add(new Species
        {
            Name = "Beholder",
            Description = "Beholders are aberrant, spherical creatures with a large central eye, massive maw, and multiple eye stalks that project magical rays.",
            SpeciesType = SpeciesTypeEnum.Aberration,
            AverageHeight = "4 to 6 feet in diameter",
            AverageWeight = "2000 to 4000 pounds",
            EyeColor = "Central eye varies, eye stalks have different colors",
            HairColor = "N/A",
            SkinColor = "Chitinous, often gray, brown, or mottled",
            DistinguishingFeatures = "Spherical body, ten eye stalks, large central eye, toothy maw, levitates naturally.",
            MagicalAbilitiesDescription = "Each eye stalk fires different magical rays (disintegration, charm, sleep, petrification, etc.). Central eye projects an anti-magic cone."
        });

        // General Fantasy Species

        speciesList.Add(new Species
        {
            Name = "Unicorn",
            Description = "Unicorns are pure, magical equine creatures with a single spiraling horn. They symbolize grace, purity, and magic in many traditions.",
            SpeciesType = SpeciesTypeEnum.Mythical,
            AverageHeight = "5 to 6 feet at shoulder",
            AverageWeight = "800 to 1200 pounds",
            EyeColor = "Commonly blue, violet, or silver",
            HairColor = "Usually white or silver, sometimes golden",
            SkinColor = "White or pale",
            DistinguishingFeatures = "Single spiraling horn, often with a flowing mane and tail, hooves that leave no tracks.",
            MagicalAbilitiesDescription = "Unicorn horns can purify poison, heal wounds, and detect evil. They can teleport short distances and are immune to most magic."
        });

        speciesList.Add(new Species
        {
            Name = "Werewolf",
            Description = "Werewolves are humans cursed or gifted with the ability to transform into wolves or wolf-human hybrids, typically during the full moon.",
            SpeciesType = SpeciesTypeEnum.Lycanthrope,
            AverageHeight = "6 to 8 feet (hybrid form)",
            AverageWeight = "200 to 400 pounds (hybrid form)",
            EyeColor = "Often yellow or amber when transformed",
            HairColor = "Varies (grows thick fur when transformed)",
            SkinColor = "Human in human form",
            DistinguishingFeatures = "Transforms during full moon, enhanced senses, vulnerable to silver, spreads lycanthropy through bites.",
            MagicalAbilitiesDescription = "Werewolves possess enhanced strength, speed, and senses. They rapidly heal from most wounds except those caused by silver."
        });

        speciesList.Add(new Species
        {
            Name = "Nymph",
            Description = "Nymphs are nature spirits tied to specific natural features such as trees, streams, or mountains. They are beautiful and magical beings.",
            SpeciesType = SpeciesTypeEnum.Fey,
            AverageHeight = "5 to 6 feet",
            AverageWeight = "80 to 130 pounds",
            EyeColor = "Varies (often green, blue, or golden)",
            HairColor = "Often matches their environment (green, blue, brown)",
            SkinColor = "Fair, sometimes with plant-like tints",
            DistinguishingFeatures = "Otherworldly beauty, connection to nature, may have plant-like features depending on type.",
            MagicalAbilitiesDescription = "Nymphs can control nature, charm mortals, become invisible in their domain, and their presence enriches the land."
        });

        speciesList.Add(new Species
        {
            Name = "Golem",
            Description = "Golems are artificial constructs created from inanimate materials (clay, stone, iron) and animated through magic or divine power.",
            SpeciesType = SpeciesTypeEnum.Construct,
            AverageHeight = "8 to 12 feet",
            AverageWeight = "500 to 5000 pounds (material dependent)",
            EyeColor = "Often glowing or none",
            HairColor = "N/A",
            SkinColor = "Varies by material (clay, stone, metal)",
            DistinguishingFeatures = "Constructed from inert materials, follows commands literally, immune to many forms of damage, often has inscriptions or sigils.",
            MagicalAbilitiesDescription = "Golems are immune to magic and most physical damage. They never tire and can have special abilities based on their material (fire immunity, etc.)."
        });


        return speciesList ?? new List<Species>();
    }

    private async Task EnsureSuperUserExistsAsync()
    {
        _logger.LogInformation("Checking for superuser...");
        var existingSuperUser = await _userRepository.GetByUsernameAsync("Admin");
        if (existingSuperUser == null)
        {
            _logger.LogInformation("Superuser not found, creating one.");
            var (hash, salt) = SecurityUtils.HashPassword("Password1234");
            var superUser = new User
            {
                Name = "Administrator",
                Description = "System Administrator",
                Username = "Admin",
                Email = "admin@glimmer.local",
                PasswordHash = hash,
                PasswordSalt = salt,
                IsSuperUser = true,
                IsActive = true,
                EmailVerified = true
            };
            await _userRepository.CreateAsync(superUser);
            _logger.LogInformation("Superuser created successfully.");
        }
        else
        {
            _logger.LogInformation("Superuser already exists.");
        }
    }
}