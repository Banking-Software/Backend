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
                        new State {Code=1, EnglishName="Koshi Pardesh", NepaliName="कोशी प्रदेश"},
                        new State {Code=2, EnglishName="Madesh Pardesh", NepaliName="मदेश प्रदेश"},
                        new State {Code=3, EnglishName="Bagmati Pardesh", NepaliName="बागमती प्रदेश"},
                        new State {Code=4, EnglishName="Gandaki Pardesh", NepaliName="गण्डकी प्रदेश"},
                        new State {Code=5, EnglishName="Lumbini Pardesh", NepaliName="लुम्बिनी प्रदेश"},
                        new State {Code=6, EnglishName="Karnali Pardesh", NepaliName="कर्णाली प्रदेश"},
                        new State {Code=7, EnglishName="Sudurpashchim Pardesh", NepaliName="सुदुरपश्चिम प्रदेश"}

                });
                await context.SaveChangesAsync();
            }

            if (!context.Casts.Any())
            {
                await context.Casts.AddRangeAsync(new List<Cast>
                {
                    new Cast(){Code=2,  NepaliName="ब्रामण", EnglishName="Brahmin"},
                    new Cast(){Code=1,  NepaliName="क्षेत्री", EnglishName="Kshatriya"},
                    new Cast(){Code=3,  NepaliName="मगर", EnglishName="Magar"},
                    new Cast(){Code=4,  NepaliName="थारु", EnglishName="Tharu"},
                    new Cast(){Code=5,  NepaliName="तमाङ्ग", EnglishName="Tamang"},
                    new Cast(){Code=6,  NepaliName="नेवार", EnglishName="Newar"},
                    new Cast(){Code=7,  NepaliName="मुसलमान", EnglishName="Muslim"},
                    new Cast(){Code=8,  NepaliName="कामी", EnglishName="Kami"},
                    new Cast(){Code=9,  NepaliName="यादव", EnglishName="Yadav"},
                    new Cast(){Code=10, NepaliName="रार्इ", EnglishName="Rai"},
                    new Cast(){Code=11, NepaliName="गुरुङ्ग", EnglishName="Gurung"},
                    new Cast(){Code=12, NepaliName="दमार्इ/धोली", EnglishName="Damai/Dholi"},
                    new Cast(){Code=13, NepaliName="लिम्वु", EnglishName="Limbu"},
                    new Cast(){Code=14, NepaliName="ठकुरी ", EnglishName="Thakuri"},
                    new Cast(){Code=15, NepaliName="सार्की", EnglishName="Sarki"},
                    new Cast(){Code=16, NepaliName="तेली", EnglishName="Teli"},
                    new Cast(){Code=17, NepaliName="चमार/हरिजन/राम", EnglishName="Chamar/Harijan/Ram"},
                    new Cast(){Code=18, NepaliName="कोर्इरी", EnglishName="Koiri"},
                    new Cast(){Code=19, NepaliName="कुर्मी", EnglishName="Kurmi"},
                    new Cast(){Code=20, NepaliName="सन्यासी/दसनामी", EnglishName="Sanyasi/Dasnam"},
                    new Cast(){Code=21, NepaliName="धानुक", EnglishName="Dhanuk"},
                    new Cast(){Code=22, NepaliName="मुसहर", EnglishName="Musahar"},
                    new Cast(){Code=23, NepaliName="दुसाध/पासवान/पासी", EnglishName="Dusadh/Paswan/Pasi"},
                    new Cast(){Code=24, NepaliName="शेर्पा", EnglishName="Sherpa"},
                    new Cast(){Code=25, NepaliName="सोनार", EnglishName="Sonar"},
                    new Cast(){Code=26, NepaliName="केवट", EnglishName="Kevat"},
                    new Cast(){Code=27, NepaliName="ब्रामण तराइ", EnglishName="Brahmin Terai"},
                    new Cast(){Code=28, NepaliName="कथवानिया", EnglishName="Kathwaniya"},
                    new Cast(){Code=29, NepaliName="घर्ति/भुजेल", EnglishName="Gharti/Bhujel"},
                    new Cast(){Code=30, NepaliName="मलाल्स", EnglishName="Malal"},
                    new Cast(){Code=31, NepaliName="कलवार", EnglishName="Kalwar"},
                    new Cast(){Code=32, NepaliName="कुमाल", EnglishName="Kumal"},
                    new Cast(){Code=33, NepaliName="हजाम/ठाकुर", EnglishName="Hajam/Thakur"},
                    new Cast(){Code=34, NepaliName="कानु", EnglishName="Kanu"},
                    new Cast(){Code=35, NepaliName="राजवशीं", EnglishName="Rajbanshi"},
                    new Cast(){Code=36, NepaliName="सुनुवार", EnglishName="Sunuwar"},
                    new Cast(){Code=37, NepaliName="सुधी", EnglishName="Sudhi"},
                    new Cast(){Code=38, NepaliName="लोसार", EnglishName="Losar"},
                    new Cast(){Code=39, NepaliName="तत्मा/तत्वा", EnglishName="Tatma/Tatwa"},
                    new Cast(){Code=40, NepaliName="खत्वे", EnglishName="Khatwe"},
                    new Cast(){Code=41, NepaliName="धोवी", EnglishName="Dhobi"},
                    new Cast(){Code=42, NepaliName="माझी", EnglishName="Majhi"},
                    new Cast(){Code=43, NepaliName="नुनिया", EnglishName="Nuniya"},
                    new Cast(){Code=44, NepaliName="गुप्ता", EnglishName="Gupta"},
                    new Cast(){Code=45, NepaliName="अन्य", EnglishName="Others"}

                });
                await context.SaveChangesAsync();
            }

            if(!context.Districts.Any())
            {
                await context.Districts.AddRangeAsync(new List<District>()
                {
                    new District(){Code=1,  NepaliName="ताप्लेजुङ", EnglishName="Taplejung"},
                    new District(){Code=2,  NepaliName="पाँचथर", EnglishName="Panchthar"},
                    new District(){Code=3,  NepaliName="ईलाम", EnglishName="Ilam"},
                    new District(){Code=4,  NepaliName="संखुवासभा", EnglishName="Sankhuwasabha"},
                    new District(){Code=5,  NepaliName="तेर्हथुम", EnglishName="Tehrathum"},
                    new District(){Code=6,  NepaliName="धनकुटा", EnglishName="Dhankuta"},
                    new District(){Code=7,  NepaliName="भोजपुर", EnglishName="Bhojpur"},
                    new District(){Code=8,  NepaliName="खोटांङ", EnglishName="Khotang"},
                    new District(){Code=9,  NepaliName="सोलुखुम्बु", EnglishName="Solukhumbu"},
                    new District(){Code=10, NepaliName="ओखलढुंगा", EnglishName="Okhaldhunga"},
                    new District(){Code=11, NepaliName="उदयपुर", EnglishName="Udayapur"},
                    new District(){Code=12, NepaliName="झापा", EnglishName="Jhapa"},
                    new District(){Code=13, NepaliName="मोरङ", EnglishName="Morang"},
                    new District(){Code=14, NepaliName="सुनसरी", EnglishName="Sunsari"},
                    new District(){Code=15, NepaliName="सप्तरी", EnglishName="Saptari"},
                    new District(){Code=16, NepaliName="सिराहा", EnglishName="Siraha"},
                    new District(){Code=17, NepaliName="धनुषा", EnglishName="Dhanusha"},
                    new District(){Code=18, NepaliName="महोतरी", EnglishName="Mahottari"},
                    new District(){Code=19, NepaliName="सर्लाही", EnglishName="Sarlahi"},
                    new District(){Code=20, NepaliName="रौतहट", EnglishName="Rautahat"},
                    new District(){Code=21, NepaliName="बारा", EnglishName="Bara"},
                    new District(){Code=22, NepaliName="पर्सा", EnglishName="Parsa"},
                    new District(){Code=23, NepaliName="दोलखा", EnglishName="Dolakha"},
                    new District(){Code=24, NepaliName="रामेछाप", EnglishName="Ramechhap"},
                    new District(){Code=25, NepaliName="सिन्धुली", EnglishName="Sindhuli"},
                    new District(){Code=26, NepaliName="काभ्रेपलान्चोक", EnglishName="Kavrepalanchok"},
                    new District(){Code=27, NepaliName="सिन्धुपाल्चोक", EnglishName="Sindhupalchok"},
                    new District(){Code=28, NepaliName="रसुवा", EnglishName="Rasuwa"},
                    new District(){Code=29, NepaliName="नुवाकोट", EnglishName="Nuwakot"},
                    new District(){Code=30, NepaliName="धादिंङ", EnglishName="Dhading"},
                    new District(){Code=31, NepaliName="चितवन", EnglishName="Chitwan"},
                    new District(){Code=32, NepaliName="भक्तपुर", EnglishName="Bhaktapur"},
                    new District(){Code=33, NepaliName="मकवानपुर", EnglishName="Makwanpur"},
                    new District(){Code=34, NepaliName="ललितपुर", EnglishName="Lalitpur"},
                    new District(){Code=35, NepaliName="काठमाण्डौ", EnglishName="Kathmandu"},
                    new District(){Code=36, NepaliName="गोर्खा", EnglishName="Gorkha"},
                    new District(){Code=37, NepaliName="लमजुङ", EnglishName="Lamjung"},
                    new District(){Code=38, NepaliName="तनहुँ", EnglishName="Tanahun"},
                    new District(){Code=39, NepaliName="कास्की", EnglishName="Kaski"},
                    new District(){Code=40, NepaliName="मनाङ", EnglishName="Manang"},
                    new District(){Code=41, NepaliName="मुस्तांङ", EnglishName="Mustang"},
                    new District(){Code=42, NepaliName="पर्वत", EnglishName="Parbat"},
                    new District(){Code=43, NepaliName="स्याङ्जा", EnglishName="Syangja"},
                    new District(){Code=44, NepaliName="म्याग्दी", EnglishName="Myagdi"},
                    new District(){Code=45, NepaliName="बाग्लुंङ", EnglishName="Baglung"},
                    new District(){Code=46, NepaliName="नवलपरासी (बरद्घाट सुस्तापूर्व)", EnglishName="Nawalparasi (Bardghat Susta East)"},
                    new District(){Code=47, NepaliName="नवलपरासी (बरद्घाट सुस्ता पश्चिम )", EnglishName="Nawalparasi (Bardghat Susta West)"},
                    new District(){Code=48, NepaliName="रुपन्देही", EnglishName="Rupandehi"},
                    new District(){Code=49, NepaliName="कपिलवस्तु", EnglishName="Kapilvastu"},
                    new District(){Code=50, NepaliName="पाल्पा", EnglishName="Palpa"},
                    new District(){Code=51, NepaliName="अर्घाखाँची", EnglishName="Arghakhanchi"},
                    new District(){Code=52, NepaliName="गुल्मी", EnglishName="Gulmi"},
                    new District(){Code=53, NepaliName="रुकुम (पूर्वी भाग)", EnglishName="Rukum (East)"},
                    new District(){Code=54, NepaliName="रोल्पा", EnglishName="Rolpa"},
                    new District(){Code=55, NepaliName="प्युठान", EnglishName="Pyuthan"},
                    new District(){Code=56, NepaliName="दाङ", EnglishName="Dang"},
                    new District(){Code=57, NepaliName="बाँके", EnglishName="Banke"},
                    new District(){Code=58, NepaliName="बर्दिया", EnglishName="Bardiya"},
                    new District(){Code=59, NepaliName="रुकुम (पश्चिम भाग)", EnglishName="Rukum (West)"},
                    new District(){Code=60, NepaliName="सल्यान", EnglishName="Salyan"},
                    new District(){Code=61, NepaliName="डोल्पा", EnglishName="Dolpa"},
                    new District(){Code=62, NepaliName="जुम्ला", EnglishName="Jumla"},
                    new District(){Code=63, NepaliName="मुगु", EnglishName="Mugu"},
                    new District(){Code=64, NepaliName="हुम्ला", EnglishName="Humla"},
                    new District(){Code=65, NepaliName="कालिकोट", EnglishName="Kalikot"},
                    new District(){Code=66, NepaliName="जाजरकोट", EnglishName="Jajarkot"},
                    new District(){Code=67, NepaliName="दैलेख", EnglishName="Dailekh"},
                    new District(){Code=68, NepaliName="सुर्खेत", EnglishName="Surkhet"},
                    new District(){Code=69, NepaliName="बाजुरा", EnglishName="Bajura"},
                    new District(){Code=70, NepaliName="बझाङ", EnglishName="Bajhang"},
                    new District(){Code=71, NepaliName="डोटी", EnglishName="Doti"},
                    new District(){Code=72, NepaliName="अछाम", EnglishName="Achham"},
                    new District(){Code=73, NepaliName="दार्चुला", EnglishName="Darchula"},
                    new District(){Code=74, NepaliName="बैतडी", EnglishName="Baitadi"},
                    new District(){Code=75, NepaliName="डडेलधुरा", EnglishName="Dadeldhura"},
                    new District(){Code=76, NepaliName="कञ्चनपुर", EnglishName="Kanchanpur"},
                    new District(){Code=77, NepaliName="कैलाली", EnglishName="Kailali"}

                });
                await context.SaveChangesAsync();
            }

            if(!context.MaritalStatuses.Any())
            {
                await context.MaritalStatuses.AddRangeAsync(new List<MaritalStatus>{
                    new MaritalStatus(){Code=1, NepaliName="विवाहित", EnglishName="Married"},
                    new MaritalStatus(){Code=2, NepaliName="अविवाहित", EnglishName="Unmarried"},
                    new MaritalStatus(){Code=3, NepaliName="विदुर", EnglishName="Divorced"},
                    new MaritalStatus(){Code=4, NepaliName="विदुवा", EnglishName="Widow"},
                    new MaritalStatus(){Code=5, NepaliName="परपाचुके", EnglishName="Separated"},
                    new MaritalStatus(){Code=6, NepaliName="छुट्टिएक", EnglishName="Annulled"}

                });
                await context.SaveChangesAsync();
            }
            if(!context.Genders.Any())
            {
                await context.Genders.AddRangeAsync(new List<Gender>()
                {
                    new Gender(){Code=1, NepaliName="पुरूष", EnglishName="Male"},
                    new Gender(){Code=2, NepaliName="महिला", EnglishName="Female"}
                });
                await context.SaveChangesAsync();
            }
        }
    }
}