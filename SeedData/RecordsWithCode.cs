using MicroFinance.DBContext;
using MicroFinance.Models.ClientSetup;
using MicroFinance.Models.RecordsWithCode;

namespace MicroFinance.SeedData  
{
    public class RecordsWithCode
    {
        public static async Task SeedRecordsWithCode(ApplicationDbContext context)
        {
            
            if (!context.States.Any())
            {
                await context.States.AddRangeAsync(new List<State>
                {
                        new State {Code=1, Title="Koshi Pardesh"},
                        new State {Code=2, Title="Madesh Pardesh"},
                        new State {Code=3, Title="Bagmati Pardesh"},
                        new State {Code=4, Title="Gandaki Pardesh"},
                        new State {Code=5, Title="Lumbini Pardesh"},
                        new State {Code=6, Title="Karnali Pardesh"},
                        new State {Code=7, Title="Sudurpashchim Pardesh"}

                });
                await context.SaveChangesAsync();
            }

            if (!context.Casts.Any())
            {
                await context.Casts.AddRangeAsync(new List<Cast>
                {
                    new Cast(){Code=1, Title="क्षेत्री"},
                    new Cast(){Code=2, Title="ब्रामण"},
                    new Cast(){Code=3, Title="मगर"},
                    new Cast(){Code=4, Title="थारु"},
                    new Cast(){Code=5, Title="तमाङ्ग"},
                    new Cast(){Code=6, Title="नेवार"},
                    new Cast(){Code=7, Title="मुसलमान"},
                    new Cast(){Code=8, Title="कामी"},
                    new Cast(){Code=9, Title="यादव"},
                    new Cast(){Code=10, Title="रार्इ"},
                    new Cast(){Code=11, Title="गुरुङ्ग"},
                    new Cast(){Code=12, Title="दमार्इ/धोली"},
                    new Cast(){Code=13, Title="लिम्वु"},
                    new Cast(){Code=14, Title="ठकुरी "},
                    new Cast(){Code=15, Title="सार्की"},
                    new Cast(){Code=16, Title="तेली"},
                    new Cast(){Code=17, Title="चमार/हरिजन/राम"},
                    new Cast(){Code=18, Title="कोर्इरी"},
                    new Cast(){Code=19, Title="कुर्मी"},
                    new Cast(){Code=20, Title="सन्यासी/दसनामी"},
                    new Cast(){Code=21, Title="धानुक"},
                    new Cast(){Code=22, Title="मुसहर"},
                    new Cast(){Code=23, Title="दुसाध/पासवान/पासी"},
                    new Cast(){Code=24, Title="शेर्पा"},
                    new Cast(){Code=25, Title="सोनार"},
                    new Cast(){Code=26, Title="केवट"},
                    new Cast(){Code=27, Title="ब्रामण तराइ"},
                    new Cast(){Code=28, Title="कथवानिया"},
                    new Cast(){Code=29, Title="घर्ति/भुजेल"},
                    new Cast(){Code=30, Title="मलाल्स"},
                    new Cast(){Code=31, Title="कलवार"},
                    new Cast(){Code=32, Title="कुमाल"},
                    new Cast(){Code=33, Title="हजाम/ठाकुर"},
                    new Cast(){Code=34, Title="कानु"},
                    new Cast(){Code=35, Title="राजवशीं"},
                    new Cast(){Code=36, Title="सुनुवार"},
                    new Cast(){Code=37, Title="सुधी"},
                    new Cast(){Code=38, Title="लोसार"},
                    new Cast(){Code=39, Title="तत्मा/तत्वा"},
                    new Cast(){Code=40, Title="खत्वे"},
                    new Cast(){Code=41, Title="धोवी"},
                    new Cast(){Code=42, Title="माझी"},
                    new Cast(){Code=43, Title="नुनिया"},
                    new Cast(){Code=44, Title="गुप्ता"},
                    new Cast(){Code=45, Title="अन्य"}
                });
                await context.SaveChangesAsync();
            }

            if(!context.Districts.Any())
            {
                await context.Districts.AddRangeAsync(new List<District>()
                {
                    new District(){Code=1, Title="ताप्लेजुङ"},
                    new District(){Code=2, Title="पाँचथर"},
                    new District(){Code=3, Title="ईलाम"},
                    new District(){Code=4, Title="संखुवासभा"},
                    new District(){Code=5, Title="तेर्हथुम"},
                    new District(){Code=6, Title="धनकुटा"},
                    new District(){Code=7, Title="भोजपुर"},
                    new District(){Code=8, Title="खोटांङ"},
                    new District(){Code=9, Title="सोलुखुम्बु"},
                    new District(){Code=10, Title="ओखलढुंगा"},
                    new District(){Code=11, Title="उदयपुर"},
                    new District(){Code=12, Title="झापा"},
                    new District(){Code=13, Title="मोरङ"},
                    new District(){Code=14, Title="सुनसरी"},
                    new District(){Code=15, Title="सप्तरी"},
                    new District(){Code=16, Title="सिराहा"},
                    new District(){Code=17, Title="धनुषा"},
                    new District(){Code=18, Title="महोतरी"},
                    new District(){Code=19, Title="सर्लाही"},
                    new District(){Code=20, Title="रौतहट"},
                    new District(){Code=21, Title="बारा"},
                    new District(){Code=22, Title="पर्सा"},
                    new District(){Code=23, Title="दोलखा"},
                    new District(){Code=24, Title="रामेछाप"},
                    new District(){Code=25, Title="सिन्धुली"},
                    new District(){Code=26, Title="काभ्रेपलान्चोक"},
                    new District(){Code=27, Title="सिन्धुपाल्चोक"},
                    new District(){Code=28, Title="रसुवा"},
                    new District(){Code=29, Title="नुवाकोट"},
                    new District(){Code=30, Title="धादिंङ"},
                    new District(){Code=31, Title="चितवन"},
                    new District(){Code=32, Title="भक्तपुर"},
                    new District(){Code=33, Title="मकवानपुर"},
                    new District(){Code=34, Title="ललितपुर"},
                    new District(){Code=35, Title="काठमाण्डौ"},
                    new District(){Code=36, Title="गोर्खा"},
                    new District(){Code=37, Title="लमजुङ"},
                    new District(){Code=38, Title="तनहुँ"},
                    new District(){Code=39, Title="कास्की"},
                    new District(){Code=40, Title="मनाङ"},
                    new District(){Code=41, Title="मुस्तांङ"},
                    new District(){Code=42, Title="पर्वत"},
                    new District(){Code=43, Title="स्याङ्जा"},
                    new District(){Code=44, Title="म्याग्दी"},
                    new District(){Code=45, Title="बाग्लुंङ"},
                    new District(){Code=46, Title="नवलपरासी (बरद्घाट सुस्तापूर्व)"},
                    new District(){Code=47, Title="नवलपरासी (बरद्घाट सुस्ता पश्चिम )"},
                    new District(){Code=48, Title="रुपन्देही"},
                    new District(){Code=49, Title="कपिलवस्तु"},
                    new District(){Code=50, Title="पाल्पा"},
                    new District(){Code=51, Title="अर्घाखाँची"},
                    new District(){Code=52, Title="गुल्मी"},
                    new District(){Code=53, Title="रुकुम (पूर्वी भाग)"},
                    new District(){Code=54, Title="रोल्पा"},
                    new District(){Code=55, Title="प्युठान"},
                    new District(){Code=56, Title="दाङ"},
                    new District(){Code=57, Title="बाँके"},
                    new District(){Code=58, Title="बर्दिया"},
                    new District(){Code=59, Title="रुकुम (पश्चिम भाग)"},
                    new District(){Code=60, Title="सल्यान"},
                    new District(){Code=61, Title="डोल्पा"},
                    new District(){Code=62, Title="जुम्ला"},
                    new District(){Code=63, Title="मुगु"},
                    new District(){Code=64, Title="हुम्ला"},
                    new District(){Code=65, Title="कालिकोट"},
                    new District(){Code=66, Title="जाजरकोट"},
                    new District(){Code=67, Title="दैलेख"},
                    new District(){Code=68, Title="सुर्खेत"},
                    new District(){Code=69, Title="बाजुरा"},
                    new District(){Code=70, Title="बझाङ"},
                    new District(){Code=71, Title="डोटी"},
                    new District(){Code=72, Title="अछाम"},
                    new District(){Code=73, Title="दार्चुला"},
                    new District(){Code=74, Title="बैतडी"},
                    new District(){Code=75, Title="डडेलधुरा"},
                    new District(){Code=76, Title="कञ्चनपुर"},
                    new District(){Code=77, Title="कैलाली"}
                });
                await context.SaveChangesAsync();
            }

            if(!context.MaritalStatuses.Any())
            {
                await context.MaritalStatuses.AddRangeAsync(new List<MaritalStatus>{
                    new MaritalStatus(){Code=1, Title="विवाहित"},
                    new MaritalStatus(){Code=2, Title="अविवाहित"},
                    new MaritalStatus(){Code=3, Title="विदुर"},
                    new MaritalStatus(){Code=4, Title="विदुवा"},
                    new MaritalStatus(){Code=5, Title="परपाचुके"},
                    new MaritalStatus(){Code=6, Title="छुट्टिएक"}
                });
                await context.SaveChangesAsync();
            }
            if(!context.Genders.Any())
            {
                await context.Genders.AddRangeAsync(new List<Gender>()
                {
                    new Gender(){Code=1, Title="पुरूष"},
                    new Gender(){Code=2, Title="महिला"}
                });
                await context.SaveChangesAsync();
            }
        }
    }
}