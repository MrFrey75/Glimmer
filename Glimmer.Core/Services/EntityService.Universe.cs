using Glimmer.Core.Models;
using Microsoft.Extensions.Logging;
using Glimmer.Core.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Glimmer.Core.Services;

/// <summary>
/// EntityService partial class - Universe Operations
/// </summary>
public partial class EntityService
{
    // Dependencies are already declared in the main EntityService.cs class

    public async Task<Universe?> CreateUniverseAsync(string name, string description, User createdBy, TimelineTypeEnum timelineType = TimelineTypeEnum.CalendarBased)
    {
        _logger.LogInformation("Creating universe '{Name}' for user {Username} (ID: {UserId}) with TimelineType: {TimelineType}",
            name, createdBy.Username, createdBy.Uuid, timelineType);

        var universe = new Universe
        {
            Uuid = Guid.NewGuid(),
            Name = name,
            Description = description,
            CreatedBy = createdBy,
            TimelineType = timelineType,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await _universeRepository.CreateAsync(universe);

        if (result != null)
        {
            _logger.LogInformation("Universe {UniverseId} created successfully by user {Username} (ID: {UserId})",
                universe.Uuid, createdBy.Username, createdBy.Uuid);

            bool needsUpdate = false;

            // --- Species Seeding ---
            var defaultSpecies = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Human",
                Description = "Default species representing humans.",
                SpeciesType = SpeciesTypeEnum.Humanoid,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "5.5 to 6",
                HeightMeasure = HeightMeasureEnum.Feet,
                AverageWeight = "130 to 220",
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "Varies widely",
                EyeColor = "Varies widely",
                HairColor = "Varies widely",
                DistinguishingFeatures = "None",
                NaturalHabitat = NaturalHabitatEnum.Terrestrial,
                GeographicDistribution = GeographicDistributionEnum.Global,
                SocialStructure = SocialStructureEnum.Varied,
                BehavioralTraits = "Highly adaptable and social",
                Diet = DietTypeEnum.Omnivore,
                AverageLifespan = "70-100 years",
                ReproductiveMethods = ReproductiveMethodEnum.Viviparous,
                GestationPeriod = "Approximately 9 months",
                OffspringPerBirth = "Typically 1",
                CommunicationMethods = CommunicationMethodEnum.Verbal,
                PredatorsAndThreats = "Varies by environment",
                ConservationStatus = "Not Applicable",
                HasMagicalAbilities = false,
                MagicalAbilitiesDescription = string.Empty,
                AdditionalNotes = "Humans are known for their diversity and adaptability."
            };

            var dog = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Dog",
                Description = "Domesticated species known for loyalty and companionship.",
                SpeciesType = SpeciesTypeEnum.Mammal,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "1 to 3",
                HeightMeasure = HeightMeasureEnum.Feet,
                AverageWeight = "20 to 150",
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "Varies widely",
                EyeColor = "Varies widely",
                HairColor = "Varies widely",
                DistinguishingFeatures = "Varies by breed",
                NaturalHabitat = NaturalHabitatEnum.Domestic,
                GeographicDistribution = GeographicDistributionEnum.Global,
                SocialStructure = SocialStructureEnum.Pack,
                BehavioralTraits = "Loyal, social, trainable",
                Diet = DietTypeEnum.Omnivore,
                AverageLifespan = "10-13 years",
                ReproductiveMethods = ReproductiveMethodEnum.Viviparous,
                GestationPeriod = "Approximately 63 days",
                OffspringPerBirth = "Typically 5-6 puppies",
                CommunicationMethods = CommunicationMethodEnum.None,
                PredatorsAndThreats = "Varies by environment",
                ConservationStatus = "Domesticated",
                HasMagicalAbilities = false,
                MagicalAbilitiesDescription = string.Empty,
                AdditionalNotes = "Dogs have been bred for various traits and purposes."
            };

            var cat = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Cat",
                Description = "Small domesticated carnivorous mammal known for agility and independence.",
                SpeciesType = SpeciesTypeEnum.Mammal,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "0.9 to 1",
                HeightMeasure = HeightMeasureEnum.Feet,
                AverageWeight = "8 to 15",
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "Varies widely",
                EyeColor = "Varies widely",
                HairColor = "Varies widely",
                DistinguishingFeatures = "Varies by breed",
                NaturalHabitat = NaturalHabitatEnum.Domestic,
                GeographicDistribution = GeographicDistributionEnum.Global,
                SocialStructure = SocialStructureEnum.Solitary,
                BehavioralTraits = "Independent, agile, territorial",
                Diet = DietTypeEnum.Carnivore,
                AverageLifespan = "12-16 years",
                ReproductiveMethods = ReproductiveMethodEnum.Viviparous,
                GestationPeriod = "Approximately 65 days",
                OffspringPerBirth = "Typically 3-5 kittens",
                CommunicationMethods = CommunicationMethodEnum.None,
                PredatorsAndThreats = "Varies by environment",
                ConservationStatus = "Domesticated",
                HasMagicalAbilities = false,
                MagicalAbilitiesDescription = string.Empty,
                AdditionalNotes = "Cats are known for their hunting skills and agility."
            };

            var horse = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Horse",
                Description = "Large domesticated herbivorous mammal known for strength and speed.",
                SpeciesType = SpeciesTypeEnum.Mammal,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "14 to 17",
                HeightMeasure = HeightMeasureEnum.Hands,
                AverageWeight = "900 to 2,200",
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "Varies widely",
                EyeColor = "Varies widely",
                HairColor = "Varies widely",
                DistinguishingFeatures = "Varies by breed",
                NaturalHabitat = NaturalHabitatEnum.Domestic,
                GeographicDistribution = GeographicDistributionEnum.Global,
                SocialStructure = SocialStructureEnum.Herd,
                BehavioralTraits = "Social, strong, fast",
                Diet = DietTypeEnum.Herbivore,
                AverageLifespan = "25-30 years",
                ReproductiveMethods = ReproductiveMethodEnum.Viviparous,
                GestationPeriod = "Approximately 11 months",
                OffspringPerBirth = "Typically 1 foal",
                CommunicationMethods = CommunicationMethodEnum.None,
                PredatorsAndThreats = "Varies by environment",
                ConservationStatus = "Domesticated",
                HasMagicalAbilities = false,
                MagicalAbilitiesDescription = string.Empty,
                AdditionalNotes = "Horses have been used for transportation, work, and sport."
            };
            
            // --- COMMON SPECIES ---

            var cow = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Cow",
                Description = "Large domesticated bovine, essential for dairy and meat production.",
                SpeciesType = SpeciesTypeEnum.Mammal,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "4 to 5",
                HeightMeasure = HeightMeasureEnum.Feet, // Shoulder height
                AverageWeight = "1,000 to 2,000",
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "Black, white, brown, or mixed",
                EyeColor = "Brown or blue",
                HairColor = "Short fur, varies by color",
                DistinguishingFeatures = "Large size, horns (sometimes polled)",
                NaturalHabitat = NaturalHabitatEnum.Terrestrial,
                GeographicDistribution = GeographicDistributionEnum.Global,
                SocialStructure = SocialStructureEnum.Herd,
                BehavioralTraits = "Gregarious, slow-moving, ruminant",
                Diet = DietTypeEnum.Herbivore,
                AverageLifespan = "15-20 years",
                ReproductiveMethods = ReproductiveMethodEnum.Viviparous,
                GestationPeriod = "Approximately 9 months",
                OffspringPerBirth = "Typically 1 calf",
                CommunicationMethods = CommunicationMethodEnum.Verbal,
                PredatorsAndThreats = "Historically large carnivores",
                ConservationStatus = "Domesticated",
                HasMagicalAbilities = false,
                MagicalAbilitiesDescription = string.Empty,
                AdditionalNotes = "Known for rumination and strong social bonds."
            };

            var chicken = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Chicken",
                Description = "Domesticated fowl kept for eggs and meat.",
                SpeciesType = SpeciesTypeEnum.Bird,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "0.5 to 1.5",
                HeightMeasure = HeightMeasureEnum.Feet,
                AverageWeight = "3 to 10",
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "Varies widely",
                EyeColor = "Black, brown, or red",
                HairColor = "Feathers, varies by color",
                DistinguishingFeatures = "Wattle and comb",
                NaturalHabitat = NaturalHabitatEnum.Domestic,
                GeographicDistribution = GeographicDistributionEnum.Global,
                SocialStructure = SocialStructureEnum.Flock,
                BehavioralTraits = "Highly social, forms a pecking order",
                Diet = DietTypeEnum.Omnivore,
                AverageLifespan = "5-10 years",
                ReproductiveMethods = ReproductiveMethodEnum.Oviparous, // Egg-laying
                GestationPeriod = "21 days (incubation)",
                OffspringPerBirth = "Varies",
                CommunicationMethods = CommunicationMethodEnum.None,
                PredatorsAndThreats = "Various mammals and birds of prey",
                ConservationStatus = "Domesticated",
                HasMagicalAbilities = false,
                MagicalAbilitiesDescription = string.Empty,
                AdditionalNotes = "Chickens are a common prey species, relying on flock defense."
            };

            var snake = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Snake",
                Description = "Legless carnivorous reptile with an elongated body, highly diverse.",
                SpeciesType = SpeciesTypeEnum.Reptile,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "2 to 10",
                HeightMeasure = HeightMeasureEnum.Feet, // Using Feet for common range of length
                AverageWeight = "0.1 to 100",
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "Varies widely (scales)",
                EyeColor = "Varies widely",
                HairColor = "None (scales)",
                DistinguishingFeatures = "Lack of limbs, scales, often camouflaged",
                NaturalHabitat = NaturalHabitatEnum.Terrestrial | NaturalHabitatEnum.Aquatic,
                GeographicDistribution = GeographicDistributionEnum.Continental, // Nearly global
                SocialStructure = SocialStructureEnum.Solitary,
                BehavioralTraits = "Predatory, cold-blooded, highly varied defenses",
                Diet = DietTypeEnum.Carnivore,
                AverageLifespan = "10-25 years",
                ReproductiveMethods = ReproductiveMethodEnum.Oviparous | ReproductiveMethodEnum.Viviparous, // Varies
                GestationPeriod = "Varies widely",
                OffspringPerBirth = "Varies widely",
                CommunicationMethods = CommunicationMethodEnum.Hissing | CommunicationMethodEnum.BodyLanguage,
                PredatorsAndThreats = "Birds of prey, larger mammals, other snakes",
                ConservationStatus = "Varies widely by species",
                HasMagicalAbilities = false,
                MagicalAbilitiesDescription = string.Empty,
                AdditionalNotes = "Some species are venomous; they are ecologically important as predators."
            };
            
            // --- MYTHICAL SPECIES ---
            
            var dragon = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Dragon",
                Description = "Gigantic, winged, fire-breathing reptile, often hoarders of treasure.",
                SpeciesType = SpeciesTypeEnum.Reptile, 
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "20 to 50",
                HeightMeasure = HeightMeasureEnum.Feet, 
                AverageWeight = "1,000 to 10,000",
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "Scales, highly varied (red, gold, black, green)",
                EyeColor = "Varies (often gold or glowing)",
                HairColor = "None (scales)",
                DistinguishingFeatures = "Wings, horns, and armor-like scales",
                NaturalHabitat = NaturalHabitatEnum.Mountain | NaturalHabitatEnum.Cave,
                GeographicDistribution = GeographicDistributionEnum.Continental,
                SocialStructure = SocialStructureEnum.Solitary,
                BehavioralTraits = "Proud, territorial, highly intelligent, and predatory",
                Diet = DietTypeEnum.Carnivore,
                AverageLifespan = "1,000+ years",
                ReproductiveMethods = ReproductiveMethodEnum.Oviparous, 
                GestationPeriod = "1-2 years (egg incubation)",
                OffspringPerBirth = "Typically 1 to 5 eggs",
                CommunicationMethods = CommunicationMethodEnum.Verbal | CommunicationMethodEnum.None | CommunicationMethodEnum.Telepathy ,
                PredatorsAndThreats = "Exceptional heroes or other dragons",
                ConservationStatus = "Mythical/Endangered",
                HasMagicalAbilities = true,
                MagicalAbilitiesDescription = "Fire breath, flight, and innate spell resistance."
            };

            var unicorn = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Unicorn",
                Description = "Noble, single-horned equine, symbolizing purity and grace.",
                SpeciesType = SpeciesTypeEnum.Mammal,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "15 to 16",
                HeightMeasure = HeightMeasureEnum.Hands,
                AverageWeight = "1,000 to 1,500",
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "White",
                EyeColor = "Blue or silver",
                HairColor = "White or silver mane and tail",
                DistinguishingFeatures = "Single, spiraled horn on the forehead",
                NaturalHabitat = NaturalHabitatEnum.Forest,
                GeographicDistribution = GeographicDistributionEnum.Continental,
                SocialStructure = SocialStructureEnum.Solitary,
                BehavioralTraits = "Shy, peaceful, drawn to purity",
                Diet = DietTypeEnum.Herbivore,
                AverageLifespan = "500 to 1,000 years",
                ReproductiveMethods = ReproductiveMethodEnum.Viviparous,
                GestationPeriod = "Approximately 11 months",
                OffspringPerBirth = "Typically 1 foal",
                CommunicationMethods = CommunicationMethodEnum.Verbal | CommunicationMethodEnum.Telepathy | CommunicationMethodEnum.None,
                PredatorsAndThreats = "Evil sorcerers, beasts of corruption",
                ConservationStatus = "Mythical/Rare",
                HasMagicalAbilities = true,
                MagicalAbilitiesDescription = "Healing touch, dispelling of darkness/poison, and limited teleportation."
            };
            
            var griffin = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Griffin",
                Description = "A majestic creature with the body of a lion and the head and wings of an eagle.",
                SpeciesType = SpeciesTypeEnum.Bird,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "8 to 10",
                HeightMeasure = HeightMeasureEnum.Feet, // Shoulder height
                AverageWeight = "500 to 1,000",
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "Golden-brown feathers, tawny fur",
                EyeColor = "Amber or yellow",
                HairColor = "Feathers and fur",
                DistinguishingFeatures = "Large wings, powerful talons, lion's hindquarters",
                NaturalHabitat = NaturalHabitatEnum.Mountain | NaturalHabitatEnum.Terrestrial,
                GeographicDistribution = GeographicDistributionEnum.Continental,
                SocialStructure = SocialStructureEnum.Pair,
                BehavioralTraits = "Regal, fiercely loyal, highly protective of young/nests",
                Diet = DietTypeEnum.Carnivore,
                AverageLifespan = "200 to 500 years",
                ReproductiveMethods = ReproductiveMethodEnum.Oviparous,
                GestationPeriod = "Varies",
                OffspringPerBirth = "Typically 1-2 chicks",
                CommunicationMethods = CommunicationMethodEnum.Verbal | CommunicationMethodEnum.None,
                PredatorsAndThreats = "None, they are often apex predators",
                ConservationStatus = "Mythical/Protected",
                HasMagicalAbilities = true,
                MagicalAbilitiesDescription = "Exceptional eyesight, resistance to physical harm, and strong territorial magic."
            };

            // --- LORD OF THE RINGS SPECIES ADDED ---

            var elf = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Elf",
                Description = "The Firstborn. Immortal, wise, and highly skilled humanoids closely attuned to nature.",
                SpeciesType = SpeciesTypeEnum.Humanoid,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "6 to 7",
                HeightMeasure = HeightMeasureEnum.Feet, 
                AverageWeight = "140 to 180",
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "Pale or fair",
                EyeColor = "Varies widely (often grey or blue)",
                HairColor = "Dark or golden, often worn long",
                DistinguishingFeatures = "Pointed ears, ethereal beauty, light step",
                NaturalHabitat = NaturalHabitatEnum.Forest | NaturalHabitatEnum.Mountain,
                GeographicDistribution = GeographicDistributionEnum.Continental,
                SocialStructure = SocialStructureEnum.Varied, // Kingdoms/Clans
                BehavioralTraits = "Patient, artistic, mournful, and deadly in combat",
                Diet = DietTypeEnum.Omnivore,
                AverageLifespan = "Immortal (unless slain in battle or waste away from grief)",
                ReproductiveMethods = ReproductiveMethodEnum.Viviparous,
                GestationPeriod = "Approximately 9 months",
                OffspringPerBirth = "Few children throughout their long lives",
                CommunicationMethods = CommunicationMethodEnum.Verbal | CommunicationMethodEnum.None,
                PredatorsAndThreats = "Orcs, Balrogs, and the fading of their world",
                ConservationStatus = "Fading",
                HasMagicalAbilities = true,
                MagicalAbilitiesDescription = "Exceptional eyesight, foresight, innate healing, and power in song/crafts to preserve."
            };

            var orc = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Orc",
                Description = "Twisted, foul humanoids corrupted from Elves by the first Dark Lord.",
                SpeciesType = SpeciesTypeEnum.Humanoid,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "5 to 6",
                HeightMeasure = HeightMeasureEnum.Feet,
                AverageWeight = "120 to 200",
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "Sallow, green, or dark brown/black",
                EyeColor = "Red or black",
                HairColor = "Sparse and black",
                DistinguishingFeatures = "Stooped posture, curved limbs, fangs",
                NaturalHabitat = NaturalHabitatEnum.Cave | NaturalHabitatEnum.Mountain,
                GeographicDistribution = GeographicDistributionEnum.Continental,
                SocialStructure = SocialStructureEnum.Varied, // Hordes/Warbands
                BehavioralTraits = "Cruel, cowardly (unless in overwhelming numbers), and subservient to a master",
                Diet = DietTypeEnum.Carnivore,
                AverageLifespan = "Unknown, often short due to internal violence or conflict",
                ReproductiveMethods = ReproductiveMethodEnum.Viviparous,
                GestationPeriod = "Varies",
                OffspringPerBirth = "Varies",
                CommunicationMethods = CommunicationMethodEnum.Verbal | CommunicationMethodEnum.Vocalizations,
                PredatorsAndThreats = "Elves, Men, Dwarves, and internal strife",
                ConservationStatus = "Widespread Menace",
                HasMagicalAbilities = false,
                MagicalAbilitiesDescription = "None. They rely on dark sorcery from their masters."
            };

            var hobbit = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Hobbit",
                Description = "A small, peace-loving branch of Men, known for their simple comfort and startling resilience.",
                SpeciesType = SpeciesTypeEnum.Humanoid,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "3 to 4",
                HeightMeasure = HeightMeasureEnum.Feet,
                AverageWeight = "80 to 120",
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "Fair or rosy-cheeked",
                EyeColor = "Brown or hazel",
                HairColor = "Curly brown hair, large hairy feet",
                DistinguishingFeatures = "Large, hairy feet; naturally stout build",
                NaturalHabitat = NaturalHabitatEnum.Terrestrial,
                GeographicDistribution = GeographicDistributionEnum.Continental,
                SocialStructure = SocialStructureEnum.Varied, // Small communities/families
                BehavioralTraits = "Hospitable, fond of food, pipe-weed, and generally averse to adventure",
                Diet = DietTypeEnum.Omnivore,
                AverageLifespan = "Around 100 years",
                ReproductiveMethods = ReproductiveMethodEnum.Viviparous,
                GestationPeriod = "Approximately 9 months",
                OffspringPerBirth = "Varies",
                CommunicationMethods = CommunicationMethodEnum.Verbal,
                PredatorsAndThreats = "None, as they are often ignored or overlooked",
                ConservationStatus = "Thriving in isolation",
                HasMagicalAbilities = false,
                MagicalAbilitiesDescription = "None, though they possess a natural 'stealth' and uncanny hardiness."
            };

            var zombie = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Zombie (The Undead)",
                Description = "A reanimated corpse, typically through a viral pathogen or dark magic, driven by a relentless hunger for living flesh.",
                SpeciesType = SpeciesTypeEnum.Undead, // Assuming you have an 'Undead' or similar enum value
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "5 to 6.5", // Varies based on original host
                HeightMeasure = HeightMeasureEnum.Feet,
                AverageWeight = "120 to 250", // Varies based on original host and decomposition
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "Grey, sickly yellow, or mottled green/brown",
                EyeColor = "Cloudy white, yellowed, or entirely missing",
                HairColor = "Thinning or falling out",
                DistinguishingFeatures = "Severe tissue damage, visible decay, shambling gait, uncoordinated movements.",
                NaturalHabitat = NaturalHabitatEnum.Terrestrial, // Wherever the host died/turned
                GeographicDistribution = GeographicDistributionEnum.Global, // Potential for widespread pandemic
                SocialStructure = SocialStructureEnum.Horde, // Driven by proximity and common goal (feeding)
                BehavioralTraits = "Aggressive, single-minded focus on feeding, slow movement (typically), highly resistant to pain.",
                Diet = DietTypeEnum.Carnivore, // Primarily focused on living tissue/brains
                AverageLifespan = "Indefinite", // Until completely destroyed or decomposed
                ReproductiveMethods = ReproductiveMethodEnum.Infection, // Bite/scratch or exposure to bodily fluids
                GestationPeriod = "Minutes to Hours", // Time taken for infection to fully turn the host
                OffspringPerBirth = "One (the infected host)",
                CommunicationMethods = CommunicationMethodEnum.None, // Moans, groans, shrieks, growls
                PredatorsAndThreats = "Humans (who organize), fire, severe blunt force trauma to the head/brain.",
                ConservationStatus = "Rapidly Spreading Threat",
                HasMagicalAbilities = false,
                MagicalAbilitiesDescription = "No true magical abilities, though some variations may be mystically animated."
            };

            var vampire = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Vampire (The Undead)",
                Description = "A reanimated corpse, typically through a viral pathogen or dark magic, driven by a relentless hunger for living flesh.",
                SpeciesType = SpeciesTypeEnum.Undead, // Assuming you have an 'Undead' or similar enum value
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "5 to 6.5", // Varies based on original host
                HeightMeasure = HeightMeasureEnum.Feet,
                AverageWeight = "120 to 250", // Varies based on original host and decomposition
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "Pale, often cold to the touch",
                EyeColor = "Varies widely (often red or black when feeding)",
                HairColor = "Varies widely",
                DistinguishingFeatures = "Fangs, sometimes claws, aversion to sunlight.",
                NaturalHabitat = NaturalHabitatEnum.Terrestrial, // Wherever the host died/turned
                GeographicDistribution = GeographicDistributionEnum.Global, // Potential for widespread pandemic
                SocialStructure = SocialStructureEnum.Varied, // Solitary to hierarchical covens
                BehavioralTraits = "Predatory, cunning, often charismatic, nocturnal.",
                Diet = DietTypeEnum.Blood, // Requires blood to survive
                AverageLifespan = "Indefinite", // Until destroyed
                ReproductiveMethods = ReproductiveMethodEnum.Infection, // Bite/scratch or exposure to bodily fluids
                GestationPeriod = "Varies", 
                OffspringPerBirth = "One (the infected host)",
                CommunicationMethods = CommunicationMethodEnum.Verbal | CommunicationMethodEnum.Telepathy | CommunicationMethodEnum.None,
                PredatorsAndThreats = "Vampire hunters, sunlight, wooden stakes through the heart.",
                ConservationStatus = "Hidden Threat",
                HasMagicalAbilities = true,
                MagicalAbilitiesDescription = "Enhanced strength, speed, healing; some possess hypnotic or shape-shifting abilities."
            };

            var sprite = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Sprite",
                Description = "A small, often winged, humanoid elemental spirit closely tied to natural elements like air or water.",
                SpeciesType = SpeciesTypeEnum.Elemental, // Assuming an 'Elemental' enum type
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "0.5 to 1",
                HeightMeasure = HeightMeasureEnum.Foot, // Or potentially Inches, depending on your scale
                AverageWeight = "Less than 1",
                WeightMeasure = WeightMeasureEnum.Pounds, // Measured in fractions of a pound or ounces
                SkinColor = "Translucent, faintly glowing, or reflecting the environment",
                EyeColor = "Vibrant shades (e.g., bright green, sky blue)",
                HairColor = "Often wispy or resembling plant matter/light",
                DistinguishingFeatures = "Small, delicate wings (like an insect's or dragonfly's); ephemeral appearance.",
                NaturalHabitat = NaturalHabitatEnum.Forest, // Typically near groves, clear streams, or meadows
                GeographicDistribution = GeographicDistributionEnum.Localized,
                SocialStructure = SocialStructureEnum.Colony, // Often found in small, coordinated groups
                BehavioralTraits = "Playful, mischievous, highly protective of their localized environment, difficult to perceive.",
                Diet = DietTypeEnum.Nectar, // Perhaps absorbing dew, pollen, or ambient energy
                AverageLifespan = "Variable / Tied to their environment's health",
                ReproductiveMethods = ReproductiveMethodEnum.Budding, // Non-traditional method suggested
                GestationPeriod = "Unknown / Rapid",
                OffspringPerBirth = "Multiple small entities",
                CommunicationMethods = CommunicationMethodEnum.Verbal, // Mostly subtle noises, chimes, or feelings
                PredatorsAndThreats = "Iron, pollution, or those actively seeking to capture magical beings.",
                ConservationStatus = "Highly Vulnerable to Industrialization",
                HasMagicalAbilities = true,
                MagicalAbilitiesDescription = "Invisibility, flight, minor control over localized nature effects (e.g., causing flowers to bloom)."
            };

            var sasquatch = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Sasquatch (Bigfoot)",
                Description = "A large, ape-like cryptid said to inhabit the forests of North America, known for its immense size and elusiveness.",
                SpeciesType = SpeciesTypeEnum.Cryptid, // A new classification for the unknown
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "6.5 to 9",
                HeightMeasure = HeightMeasureEnum.Foot,
                AverageWeight = "400 to 800",
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "Dark, thick hide visible beneath heavy fur",
                EyeColor = "Dark brown or black",
                HairColor = "Thick, long, matted brown or reddish-brown fur covering the entire body",
                DistinguishingFeatures = "Extreme height and mass, bipedal gait, distinct musky odor, massive footprints.",
                NaturalHabitat = NaturalHabitatEnum.Forest,
                GeographicDistribution = GeographicDistributionEnum.Continental, // Focus on Pacific Northwest, but sightings are widespread
                SocialStructure = SocialStructureEnum.Solitary, // Rarely seen in groups
                BehavioralTraits = "Highly intelligent, extremely cautious of humans, nocturnal, powerful.",
                Diet = DietTypeEnum.Omnivore, // Primarily vegetarian, but opportunistic
                AverageLifespan = "Unknown / Potentially long",
                ReproductiveMethods = ReproductiveMethodEnum.Viviparous,
                GestationPeriod = "Unknown",
                OffspringPerBirth = "Likely one",
                CommunicationMethods = CommunicationMethodEnum.Vocalizations, // Deep calls, wood knocks, scent marking
                PredatorsAndThreats = "Humans (researchers/hunters), extreme cold.",
                ConservationStatus = "Unconfirmed / Mythological Status",
                HasMagicalAbilities = false,
                MagicalAbilitiesDescription = "Possesses an exceptional natural camouflage/stealth ability, allowing for easy evasion."
            };

            var yeti = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Yeti (Abominable Snowman)",
                Description = "A large, ape-like cryptid reported to inhabit the high-altitude, snowy regions of the Himalayas.",
                SpeciesType = SpeciesTypeEnum.Cryptid,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "5 to 7",
                HeightMeasure = HeightMeasureEnum.Feet,
                AverageWeight = "300 to 500",
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "Dark, covered heavily by fur",
                EyeColor = "Dark brown or black",
                HairColor = "Very thick, long, shaggy white or reddish-brown fur, providing extreme insulation.",
                DistinguishingFeatures = "Massive build, long arms, thick white fur adapted for cold, generally bipedal.",
                NaturalHabitat = NaturalHabitatEnum.AlpineTundra, // High-altitude snow and ice
                GeographicDistribution = GeographicDistributionEnum.Regional, // Primarily the Himalayan mountain range
                SocialStructure = SocialStructureEnum.Solitary,
                BehavioralTraits = "Extremely elusive, adapted to harsh cold, potential for great strength.",
                Diet = DietTypeEnum.Omnivore, // Likely hunts mountain goats/sheep or scavenges
                AverageLifespan = "Unknown",
                ReproductiveMethods = ReproductiveMethodEnum.Viviparous,
                GestationPeriod = "Unknown",
                OffspringPerBirth = "Likely one",
                CommunicationMethods = CommunicationMethodEnum.Vocalizations, // Howls or deep calls adapted for mountain echoes
                PredatorsAndThreats = "Extreme environmental conditions, potential rare encounters with specialized predators.",
                ConservationStatus = "Unconfirmed / Mythological Status",
                HasMagicalAbilities = false,
                MagicalAbilitiesDescription = "Exceptional natural thermal regulation and stealth in snowy conditions."
            };

            var ghost = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Ghost (Specter/Shade)",
                Description = "The lingering consciousness or soul of a deceased entity, typically bound to a specific place, object, or emotional tether.",
                SpeciesType = SpeciesTypeEnum.Ethereal, // Assuming an 'Ethereal' or 'Spirit' enum type
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "Variable",
                HeightMeasure = HeightMeasureEnum.Undefined, // Or set to 'Feet' and use a realistic range
                AverageWeight = "Near Zero", // Effectively massless or measured in ectoplasm/energy
                WeightMeasure = WeightMeasureEnum.Kilograms, // Or set to 'Grams' or 'None'
                SkinColor = "Translucent, smoky, or completely absent",
                EyeColor = "Glowing points of light, sockets, or absent",
                HairColor = "Often wispy, resembling residual energy or tied to past appearance",
                DistinguishingFeatures = "Ability to pass through solid matter, temperature drops, audible phenomena, visible spectral form.",
                NaturalHabitat = NaturalHabitatEnum.BoundLocation, // Tied to where the death/event occurred
                GeographicDistribution = GeographicDistributionEnum.Localized,
                SocialStructure = SocialStructureEnum.Solitary, // Can interact, but usually one dominant presence
                BehavioralTraits = "Driven by unfinished business, grief, or trauma; behavior ranges from passive to poltergeist-level activity.",
                Diet = DietTypeEnum.EnergyAbsorption, // Feeds on emotional energy or ambient spiritual residue
                AverageLifespan = "Indefinite (Until Rest/Release)",
                ReproductiveMethods = ReproductiveMethodEnum.None, // They are remnants, not progenitors
                GestationPeriod = "N/A",
                OffspringPerBirth = "Zero",
                CommunicationMethods = CommunicationMethodEnum.Telepathy,// Often non-verbal or through ambient sound effects
                PredatorsAndThreats = "Exorcism, spiritual cleansing, strong emotional release/resolution.",
                ConservationStatus = "Common in certain historical/traumatized locations",
                HasMagicalAbilities = true,
                MagicalAbilitiesDescription = "Incorporeality, levitation, manipulation of temperature/minor objects (poltergeist activity)."
            };

            var revenant = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Revenant",
                Description = "A deceased individual, often with a powerful, singular will, returned from the grave specifically to seek vengeance or complete a major, unresolved task.",
                SpeciesType = SpeciesTypeEnum.Undead, 
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "Variable (matches original host)",
                HeightMeasure = HeightMeasureEnum.Feet,
                AverageWeight = "Variable (matches original host, may decrease with decay)",
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "Pallid, decomposed, or dried (depending on decay state)",
                EyeColor = "Hollow, glowing with malevolent light, or matching original host",
                HairColor = "Thinning or matted, matching original host",
                DistinguishingFeatures = "Signs of death (wounds, pallor), focused, relentless pursuit, often retains intelligence and fighting skill.",
                NaturalHabitat = NaturalHabitatEnum.BoundLocation, // Often returns to the site of its death or the residence of its target
                GeographicDistribution = GeographicDistributionEnum.Localized,
                SocialStructure = SocialStructureEnum.Solitary, // Driven by personal, singular purpose
                BehavioralTraits = "Highly intelligent, relentless, driven by hatred/vengeance, often physically stronger than in life.",
                Diet = DietTypeEnum.None, // Has no need for sustenance, driven by spiritual energy/will
                AverageLifespan = "Variable (Until Vengeance is Achieved or Destroyed)",
                ReproductiveMethods = ReproductiveMethodEnum.None,
                GestationPeriod = "N/A",
                OffspringPerBirth = "Zero",
                CommunicationMethods = CommunicationMethodEnum.Verbal, // Can often speak and plot, unlike a Zombie
                PredatorsAndThreats = "Holy magic, fire, removal of the object/person holding them to the world, severe blunt force.",
                ConservationStatus = "Rare and isolated occurrences",
                HasMagicalAbilities = true,
                MagicalAbilitiesDescription = "Possesses unnatural strength/durability; may exhibit minor necromantic influence or fear induction."
            };

            var sandworm = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Sandworm (Shai-Hulud)",
                Description = "A colossal, serpentine invertebrate native to extreme desert environments, known for traveling beneath the sand and its immense destructive power.",
                SpeciesType = SpeciesTypeEnum.Invertebrate, 
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "Variable / Subterranean",
                HeightMeasure = HeightMeasureEnum.Undefined,
                AverageWeight = "Millions", // Estimated mass for a large one
                WeightMeasure = WeightMeasureEnum.Kilograms,
                SkinColor = "Dark, rocky, or heavily segmented brown/grey hide",
                EyeColor = "None (Blind, relying on vibration)",
                HairColor = "N/A",
                DistinguishingFeatures = "Vast, concentric rings of sharp, crystalline teeth; massive size; ability to navigate and tunnel through deep sand.",
                NaturalHabitat = NaturalHabitatEnum.Desert, // Assuming a non-Earth origin like Arrakis
                GeographicDistribution = GeographicDistributionEnum.Planetary, // Found across the planetary desert
                SocialStructure = SocialStructureEnum.Solitary,
                BehavioralTraits = "Aggressive, territorial, attracted by rhythmic vibration, extremely long lifespan.",
                Diet = DietTypeEnum.FilterFeeder, // Feeds on small life forms and 'spice' in the sand
                AverageLifespan = "Millennia",
                ReproductiveMethods = ReproductiveMethodEnum.Larval, // Complex life cycle involving 'sand trout'
                GestationPeriod = "N/A",
                OffspringPerBirth = "Varies (via larval stage)",
                CommunicationMethods = CommunicationMethodEnum.Vibratory, // Primarily through seismic activity
                PredatorsAndThreats = "None in its native environment; vulnerable to flooding/water.",
                ConservationStatus = "Dominant Species",
                HasMagicalAbilities = true,
                MagicalAbilitiesDescription = "Incredibly tough hide; its internal chemicals are linked to powerful psychotropic substances (spice)."
            };

            var shimmer = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Shimmer",
                Description = "A tiny, liminal entity of pure, refracted light and mischievous will; a photonic being existing where imagination meets reality.",
                SpeciesType = SpeciesTypeEnum.Ethereal, // Continuing with the 'Ethereal' type from Ghost
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "0.01 to 0.1",
                HeightMeasure = HeightMeasureEnum.Foot, // Effectively the size of a dust mote or tiny insect
                AverageWeight = "Zero",
                WeightMeasure = WeightMeasureEnum.None, // Truly massless or near-massless
                SkinColor = "N/A (Pure light)",
                EyeColor = "N/A",
                HairColor = "N/A",
                DistinguishingFeatures = "Constant flickering and refraction; visibility only at the edge of vision; emits color pulses.",
                NaturalHabitat = NaturalHabitatEnum.LiminalSpaces, // Where reality and imagination blur (dawn, dreams, old libraries)
                GeographicDistribution = GeographicDistributionEnum.Localized, // Found near 'leaks' of imagination
                SocialStructure = SocialStructureEnum.Colony, // Gathers in clusters but operates individually
                BehavioralTraits = "Curious, playful, philosophical, prone to experimentation and harmless pranks (e.g., rearranging dust motes).",
                Diet = DietTypeEnum.EnergyAbsorption, // Feeds on imagination, wonder, and emotional energy
                AverageLifespan = "Effectively Immortal", // More like a program that updates than a biological life
                ReproductiveMethods = ReproductiveMethodEnum.Emergence, // Evolve through 'whim'; emerge from concentrated wonder/emotion
                GestationPeriod = "N/A",
                OffspringPerBirth = "Varies",
                CommunicationMethods = CommunicationMethodEnum.LuminscentPulse, // Chromatic Morse code carrying feelings
                PredatorsAndThreats = "Apathy, absolute rationalism, environments devoid of wonder, total darkness.",
                ConservationStatus = "Dependent on Human Imagination",
                HasMagicalAbilities = true,
                MagicalAbilitiesDescription = "Causes temporary **déjà vu** or warmth; able to gently tug at dreams and thoughts; subtle object manipulation."
            };

            var bat = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Bat (Chiroptera)",
                Description = "The only mammal naturally capable of sustained flight, characterized by wings formed from a membrane stretched between elongated finger bones and the body.",
                SpeciesType = SpeciesTypeEnum.Mammal, // A new, biologically accurate type
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "0.15 to 0.4", // Small to medium size in feet, depending on species
                HeightMeasure = HeightMeasureEnum.Foot,
                AverageWeight = "0.005 to 3.5", // Very light, depending on species (e.g., Kitti's hog-nosed bat to flying fox)
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "Dark grey or brown hide under fur",
                EyeColor = "Dark brown or black",
                HairColor = "Soft fur, typically brown, grey, or black",
                DistinguishingFeatures = "Large, membranous wings; use of **echolocation** (microbats); nocturnal activity.",
                NaturalHabitat = NaturalHabitatEnum.Terrestrial, // Caves, trees, buildings, rainforests
                GeographicDistribution = GeographicDistributionEnum.Global, // Found on every continent except Antarctica
                SocialStructure = SocialStructureEnum.Colony, // Often forming large, dense groups
                BehavioralTraits = "Nocturnal, typically social, utilize echolocation for navigation and hunting (microbats).",
                Diet = DietTypeEnum.Omnivore, // Varies greatly: insectivore, frugivore, nectarivore, carnivore, or sanguivore
                AverageLifespan = "20 to 30 years",
                ReproductiveMethods = ReproductiveMethodEnum.Viviparous,
                GestationPeriod = "40 days to 6 months (varies by species)",
                OffspringPerBirth = "Typically one pup",
                CommunicationMethods = CommunicationMethodEnum.Verbal, // Chirps and calls (both audible and ultrasonic)
                PredatorsAndThreats = "Owls, snakes, raccoons, diseases (like white-nose syndrome).",
                ConservationStatus = "Varies widely (Many species are threatened or endangered)",
                HasMagicalAbilities = false,
                MagicalAbilitiesDescription = "None, though echolocation is a highly specialized biological trait."
            };

            var butterfly = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Butterfly (Lepidoptera)",
                Description = "A diurnal insect known for its bright, often patterned wings, undergoing complete metamorphosis from larva (caterpillar) to pupa (chrysalis) before emerging as an adult.",
                SpeciesType = SpeciesTypeEnum.Insect, // A new, biologically accurate type
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "0.08 to 0.4", // Small size, measured in feet (depending on wingspan)
                HeightMeasure = HeightMeasureEnum.Foot,
                AverageWeight = "0.0001 to 0.001", // Very low weight
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "N/A (Exoskeleton)",
                EyeColor = "Dark compound eyes",
                HairColor = "N/A (Covered in tiny scales)",
                DistinguishingFeatures = "Large, brightly colored, scale-covered wings; clubbed antennae; specialized proboscis for feeding.",
                NaturalHabitat = NaturalHabitatEnum.Terrestrial, // Meadows, forests, gardens, tropical areas
                GeographicDistribution = GeographicDistributionEnum.Global, // Widespread, except extreme cold
                SocialStructure = SocialStructureEnum.Solitary, // Generally solitary, except during mating or migration
                BehavioralTraits = "Diurnal (daytime active); relies on mimicry and camouflage; strong migratory instinct in some species.",
                Diet = DietTypeEnum.Nectar, // Feeds primarily on nectar; larvae are herbivores (leaf-eaters)
                AverageLifespan = "2 weeks to 1 year (mostly spent as a pupa/larva)",
                ReproductiveMethods = ReproductiveMethodEnum.Oviparous, // Laying eggs
                GestationPeriod = "N/A (Metamorphosis cycle)",
                OffspringPerBirth = "Varies (Large clutch of eggs)",
                CommunicationMethods = CommunicationMethodEnum.ChemicalSignals, // Pheromones, visual display
                PredatorsAndThreats = "Birds, spiders, wasps, habitat destruction, pesticide use.",
                ConservationStatus = "Varies widely (Many species threatened)",
                HasMagicalAbilities = false,
                MagicalAbilitiesDescription = "None, though migration patterns are biologically remarkable."
            };

            var ent = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Ent (Onodrim)",
                Description = "A large, ancient sentient being resembling a tree, known as a 'Shepherd of the Trees' and characterized by immense strength and slow, deliberate actions.",
                SpeciesType = SpeciesTypeEnum.Guardian, // A new type for highly intelligent, protective beings
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "12 to 14",
                HeightMeasure = HeightMeasureEnum.Foot,
                AverageWeight = "4000 to 8000",
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "Bark-like (Grey, brown, or green-hued)",
                EyeColor = "Deep brown or bright green, often slow to blink",
                HairColor = "Moss, leaves, or thick, woody branches",
                DistinguishingFeatures = "Tree-like appearance, tough bark-like skin, deep booming voice, incredible strength.",
                NaturalHabitat = NaturalHabitatEnum.Terrestrial, // Deep, old-growth forests
                GeographicDistribution = GeographicDistributionEnum.Regional, // Very localized to ancient forests
                SocialStructure = SocialStructureEnum.SmallCommunity, // 'Entmoots' or gatherings of very few individuals
                BehavioralTraits = "Slow to anger and act ('Hasty' is a grave insult), extremely patient, devoted to the protection of trees.",
                Diet = DietTypeEnum.Photosynthesis, // Primarily draws sustenance from the environment
                AverageLifespan = "Millennia / Effectively Immortal",
                ReproductiveMethods = ReproductiveMethodEnum.Spiritual, // Reproduction process is complex and largely unknown
                GestationPeriod = "N/A",
                OffspringPerBirth = "N/A (Entwives were lost)",
                CommunicationMethods = CommunicationMethodEnum.Verbal, // 'Entish' - a slow, deliberate language
                PredatorsAndThreats = "Fire, axes, large-scale industrial destruction (like Orcs/Isengard).",
                ConservationStatus = "Critically Endangered",
                HasMagicalAbilities = true,
                MagicalAbilitiesDescription = "Possesses incredible natural fortitude, and the ability to command local trees and plant life."
            };

            var cyborg = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Cyborg (Cybernetic Organism)",
                Description = "A being combining organic and biomechatronic parts, often to enhance natural capabilities beyond human limits.",
                SpeciesType = SpeciesTypeEnum.AugmentedHumanoid, // A new type for technological enhancement
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "5 to 7", // Typically based on the original human or host
                HeightMeasure = HeightMeasureEnum.Foot,
                AverageWeight = "150 to 350", // Variable due to the weight of implanted cybernetics and power sources
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "Variable (Often human-like, sometimes covered by synthetic plating)",
                EyeColor = "Variable (Often enhanced with digital optics, glowing)",
                HairColor = "Variable",
                DistinguishingFeatures = "Visible synthetic or metallic components, enhanced strength/endurance, often requires external power/recharge.",
                NaturalHabitat = NaturalHabitatEnum.Terrestrial, // Primarily urban, industrial, or space environments
                GeographicDistribution = GeographicDistributionEnum.Global,
                SocialStructure = SocialStructureEnum.Varied, // Depends on purpose (military, civilian, solitary agent)
                BehavioralTraits = "Highly capable, precise, potentially susceptible to hacking or system failure; retains core human emotions/logic.",
                Diet = DietTypeEnum.Omnivore, // Requires both organic sustenance and energy for cybernetics
                AverageLifespan = "Variable / Extended", // Depends on maintenance and quality of organic parts
                ReproductiveMethods = ReproductiveMethodEnum.Viviparous, // Based on organic host
                GestationPeriod = "Approximately 9 months",
                OffspringPerBirth = "Varies",
                CommunicationMethods = CommunicationMethodEnum.Verbal | CommunicationMethodEnum.DataTransfer, // Can interface directly with computers
                PredatorsAndThreats = "EMP weapons, specialized hackers, system failures, highly advanced military units.",
                ConservationStatus = "Growing in advanced technological societies",
                HasMagicalAbilities = false,
                MagicalAbilitiesDescription = "Possesses technologically derived 'powers' like infrared vision, superhuman strength, or complex data processing."
            };

            var kraken = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Kraken",
                Description = "A gargantuan cephalopod-like sea monster, legendary for attacking ships and capable of pulling vessels and sailors to the ocean floor.",
                SpeciesType = SpeciesTypeEnum.Cryptid, // A new type for massive sea monsters
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "Variable (Hundreds)", // Length from end to end, measured in feet
                HeightMeasure = HeightMeasureEnum.Foot,
                AverageWeight = "Tens of Thousands", // Estimated mass
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "Reddish-brown, dark green, or mottled grey",
                EyeColor = "Massive, saucer-sized, dark yellow or green",
                HairColor = "N/A (Soft, pliable mantle)",
                DistinguishingFeatures = "Colossal size; multiple massive tentacles equipped with suckers and hooks; ability to generate whirlpools.",
                NaturalHabitat = NaturalHabitatEnum.SaltwaterAquatic, // Deep ocean trenches
                GeographicDistribution = GeographicDistributionEnum.Regional, // Historically the North Atlantic/Nordic Seas
                SocialStructure = SocialStructureEnum.Solitary, // Highly isolated and territorial
                BehavioralTraits = "Aggressive, territorial, typically passive until disturbed, capable of overwhelming nautical vessels.",
                Diet = DietTypeEnum.Carnivore, // Feeds on marine life and any unfortunate vessel/crew
                AverageLifespan = "Unknown / Millennia",
                ReproductiveMethods = ReproductiveMethodEnum.Oviparous, // Laying giant eggs (speculative)
                GestationPeriod = "Unknown",
                OffspringPerBirth = "Unknown",
                CommunicationMethods = CommunicationMethodEnum.None, // Seismic vibrations, deep-sea rumbles
                PredatorsAndThreats = "None, due to size and depth, save for other mythical beings.",
                ConservationStatus = "Unconfirmed / Mythological Status",
                HasMagicalAbilities = true,
                MagicalAbilitiesDescription = "Ability to generate powerful whirlpools and move with impossible speed for its size; possesses extreme camouflage."
            };
            
            var octopus = new Species
            {
                Uuid = Guid.NewGuid(),
                Name = "Octopus",
                Description = "A highly intelligent, eight-limbed marine mollusk of the order Octopoda, known for its soft body, excellent camouflage, and problem-solving skills.",
                SpeciesType = SpeciesTypeEnum.Mollusk, // A new, appropriate biological type
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                AverageHeight = "1 to 3", // Measured in total arm span, depending on species
                HeightMeasure = HeightMeasureEnum.Foot,
                AverageWeight = "5 to 30", // Varies greatly by species
                WeightMeasure = WeightMeasureEnum.Pounds,
                SkinColor = "Highly changeable (chromatophores), typically reddish-brown, grey, or mottled",
                EyeColor = "Dark and sophisticated (similar to vertebrates)",
                HairColor = "N/A",
                DistinguishingFeatures = "Eight highly flexible arms (tentacles) with suckers; three hearts; blue blood; ability to squeeze into tight spaces.",
                NaturalHabitat = NaturalHabitatEnum.SaltwaterAquatic, // Coastal waters, coral reefs, sea floor
                GeographicDistribution = GeographicDistributionEnum.Global, // Found in all of the world's oceans
                SocialStructure = SocialStructureEnum.Solitary, // Generally isolated and highly territorial
                BehavioralTraits = "Highly intelligent, excellent problem-solvers, masters of camouflage, utilizes jets of ink for defense.",
                Diet = DietTypeEnum.Carnivore, // Feeds on crabs, fish, and other crustaceans
                AverageLifespan = "1 to 5 years (short, often ending after reproduction)",
                ReproductiveMethods = ReproductiveMethodEnum.Oviparous, // Laying eggs
                GestationPeriod = "Varies (Female protects eggs until hatching)",
                OffspringPerBirth = "Hundreds to Thousands",
                CommunicationMethods = CommunicationMethodEnum.VisualSignals | CommunicationMethodEnum.ChemicalSignals, // Uses skin color changes and posture
                PredatorsAndThreats = "Sharks, eels, seals, habitat destruction, pollution.",
                ConservationStatus = "Varies widely (Most are common, some are vulnerable)",
                HasMagicalAbilities = false,
                MagicalAbilitiesDescription = "None, though its camouflage and intelligence are biologically remarkable."
            };


            // **FIX:** Ensure the Species list is initialized and add all defined species.
            if (result.Species == null)
            {
                result.Species = new List<Species>();
            }
            result.Species.Add(defaultSpecies);
            result.Species.Add(dog);
            result.Species.Add(cat);
            result.Species.Add(horse);
            result.Species.Add(cow);
            result.Species.Add(chicken);
            result.Species.Add(snake);
            result.Species.Add(dragon);
            result.Species.Add(unicorn);
            result.Species.Add(griffin);
            // --- ADDING LORD OF THE RINGS SPECIES ---
            result.Species.Add(elf);
            result.Species.Add(orc);
            result.Species.Add(hobbit);

            result.Species.Add(zombie);
            result.Species.Add(vampire);
            result.Species.Add(sprite);
            result.Species.Add(sasquatch);
            result.Species.Add(yeti);
            result.Species.Add(ghost);
            result.Species.Add(revenant);
            result.Species.Add(sandworm);
            result.Species.Add(shimmer);
            result.Species.Add(bat);
            result.Species.Add(butterfly);
            result.Species.Add(ent);
            result.Species.Add(cyborg);
            result.Species.Add(kraken);
            result.Species.Add(octopus);
            _logger.LogInformation("Added default species to universe {UniverseId}", universe.Uuid);

            
            needsUpdate = true;

            // Create anchor event if using relative dating
            if (timelineType == TimelineTypeEnum.RelativeDating)
            {
                _logger.LogInformation("Creating anchor event for RelativeDating universe {UniverseId}", universe.Uuid);

                var anchorEvent = new TimelineEvent
                {
                    Uuid = Guid.NewGuid(),
                    Name = "Anchor Event",
                    Description = "Primary anchor event for relative dating. This event cannot be deleted.",
                    EventType = TimelineEventTypeEnum.Other,
                    IsAnchorEvent = true,
                    UseCalendarDate = true,
                    Year = 0,
                    Era = "Year Zero",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                if (result.TimelineEvents == null)
                {
                    result.TimelineEvents = new List<TimelineEvent>();
                }
                result.TimelineEvents.Add(anchorEvent);
                needsUpdate = true;

                _logger.LogInformation("Anchor event {EventId} created for universe {UniverseId}", anchorEvent.Uuid, universe.Uuid);
            }

            // Consolidate updates into a single call for efficiency.
            if (needsUpdate)
            {
                await _universeRepository.UpdateAsync(result);
            }
        }
        else
        {
            _logger.LogError("Failed to create universe '{Name}' for user {Username} (ID: {UserId})",
                name, createdBy.Username, createdBy.Uuid);
        }

        return result;
    }

    public async Task<Universe?> GetUniverseByIdAsync(Guid universeId)
    {
        return await _universeRepository.GetByIdAsync(universeId);
    }

    public async Task<List<Universe>> GetUniversesByUserAsync(Guid userId)
    {
        return await _universeRepository.GetByUserIdAsync(userId);
    }

    public async Task<List<Universe>> GetAllUniversesAsync()
    {
        return await _universeRepository.GetAllAsync();
    }

    public async Task<bool> UpdateUniverseAsync(Universe universe)
    {
        var existing = await _universeRepository.GetByIdAsync(universe.Uuid);
        if (existing == null)
        {
            _logger.LogWarning("Universe update failed: Universe {UniverseId} not found", universe.Uuid);
            return false;
        }

        _logger.LogInformation("Updating universe {UniverseId} '{Name}'", universe.Uuid, universe.Name);

        existing.Name = universe.Name;
        existing.Description = universe.Description;
        // TimelineType is immutable after creation - do NOT update it
        existing.UpdatedAt = DateTime.UtcNow;

        var result = await _universeRepository.UpdateAsync(existing);

        if (result)
        {
            _logger.LogInformation("Universe {UniverseId} updated successfully", universe.Uuid);
        }

        return result;
    }

    public async Task<bool> DeleteUniverseAsync(Guid universeId)
    {
        var universe = await _universeRepository.GetByIdAsync(universeId);
        if (universe == null)
        {
            _logger.LogWarning("Universe deletion failed: Universe {UniverseId} not found", universeId);
            return false;
        }

        _logger.LogInformation("Deleting universe {UniverseId} '{Name}'", universeId, universe.Name);

        var result = await _universeRepository.DeleteAsync(universeId);

        if (result)
        {
            _logger.LogInformation("Universe {UniverseId} deleted successfully", universeId);
        }

        return result;
    }
}