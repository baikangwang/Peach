namespace Peach.Downloader.TingShu520
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    using Peach.Downloader.Models;

    public class Downloader : Peach.Downloader.Downloader
    {
        private static Downloader _tingshu520 = new Downloader();

        public static Downloader TingShu520
        {
            get
            {
                return _tingshu520;
            }
        }

        public IList<string> CHAPTERS = new List<string>()
                                               {
                                                   "http://www.520tingshu.com/book/book1091.html",// home page
                                               };

        private IList<int> SEEDLIBRARY = new List<string>()
                                           {
                                               "\u7B2C\u4E00\u5B63001$46481154$tudou",
                                               "\u7B2C\u4E00\u5B63002$46481156$tudou",
                                               "\u7B2C\u4E00\u5B63003$46481159$tudou",
                                               "\u7B2C\u4E00\u5B63004$46481160$tudou",
                                               "\u7B2C\u4E00\u5B63005$46481164$tudou",
                                               "\u7B2C\u4E00\u5B63006$46481165$tudou",
                                               "\u7B2C\u4E00\u5B63007$46481167$tudou",
                                               "\u7B2C\u4E00\u5B63008$46481171$tudou",
                                               "\u7B2C\u4E00\u5B63009$46481174$tudou",
                                               "\u7B2C\u4E00\u5B63010$46481179$tudou",
                                               "\u7B2C\u4E00\u5B63011$46481182$tudou",
                                               "\u7B2C\u4E00\u5B63012$46481185$tudou",
                                               "\u7B2C\u4E00\u5B63013$46481186$tudou",
                                               "\u7B2C\u4E00\u5B63014$46481189$tudou",
                                               "\u7B2C\u4E00\u5B63015$46481190$tudou",
                                               "\u7B2C\u4E00\u5B63016$46481351$tudou",
                                               "\u7B2C\u4E00\u5B63017$46481353$tudou",
                                               "\u7B2C\u4E00\u5B63018$46481355$tudou",
                                               "\u7B2C\u4E00\u5B63019$46481357$tudou",
                                               "\u7B2C\u4E00\u5B63020$46481359$tudou",
                                               "\u7B2C\u4E00\u5B63021$46481361$tudou",
                                               "\u7B2C\u4E00\u5B63022$46481363$tudou",
                                               "\u7B2C\u4E00\u5B63023$46481366$tudou",
                                               "\u7B2C\u4E00\u5B63024$46481368$tudou",
                                               "\u7B2C\u4E00\u5B63025$46481369$tudou",
                                               "\u7B2C\u4E00\u5B63026$46481370$tudou",
                                               "\u7B2C\u4E00\u5B63027$46481371$tudou",
                                               "\u7B2C\u4E00\u5B63028$46481373$tudou",
                                               "\u7B2C\u4E00\u5B63029$46481374$tudou",
                                               "\u7B2C\u4E00\u5B63030$46481376$tudou",
                                               "\u7B2C\u4E00\u5B63031$46481378$tudou",
                                               "\u7B2C\u4E00\u5B63032$46481379$tudou",
                                               "\u7B2C\u4E00\u5B63033$46481382$tudou",
                                               "\u7B2C\u4E00\u5B63034$46481385$tudou",
                                               "\u7B2C\u4E00\u5B63035$46481386$tudou",
                                               "\u7B2C\u4E00\u5B63036$46481388$tudou",
                                               "\u7B2C\u4E00\u5B63037$46481389$tudou",
                                               "\u7B2C\u4E00\u5B63038$46481390$tudou",
                                               "\u7B2C\u4E00\u5B63039$46481392$tudou",
                                               "\u7B2C\u4E00\u5B63040$46481393$tudou",
                                               "\u7B2C\u4E00\u5B63041$46481395$tudou",
                                               "\u7B2C\u4E00\u5B63042$46481396$tudou",
                                               "\u7B2C\u4E00\u5B63043$46481397$tudou",
                                               "\u7B2C\u4E00\u5B63044$46481398$tudou",
                                               "\u7B2C\u4E00\u5B63045$46481401$tudou",
                                               "\u7B2C\u4E00\u5B63046$46481402$tudou",
                                               "\u7B2C\u4E00\u5B63047$46481403$tudou",
                                               "\u7B2C\u4E00\u5B63048$46481406$tudou",
                                               "\u7B2C\u4E00\u5B63049$46481408$tudou",
                                               "\u7B2C\u4E00\u5B63050$46481409$tudou",
                                               "\u7B2C\u4E00\u5B63051$46481410$tudou",
                                               "\u7B2C\u4E00\u5B63052$46481411$tudou",
                                               "\u7B2C\u4E00\u5B63053$46481412$tudou",
                                               "\u7B2C\u4E00\u5B63054$46481413$tudou",
                                               "\u7B2C\u4E00\u5B63055$46481418$tudou",
                                               "\u7B2C\u4E00\u5B63056$46481420$tudou",
                                               "\u7B2C\u4E00\u5B63057$46481422$tudou",
                                               "\u7B2C\u4E00\u5B63058$46481424$tudou",
                                               "\u7B2C\u4E00\u5B63059$46481427$tudou",
                                               "\u7B2C\u4E00\u5B63060$46481429$tudou",
                                               "\u7B2C\u4E00\u5B63061$46481579$tudou",
                                               "\u7B2C\u4E00\u5B63062$46481737$tudou",
                                               "\u7B2C\u4E00\u5B63063$46481888$tudou",
                                               "\u7B2C\u4E00\u5B63064$46482034$tudou",
                                               "\u7B2C\u4E00\u5B63065$46482194$tudou",
                                               "\u7B2C\u4E00\u5B63066$46517669$tudou",
                                               "\u7B2C\u4E00\u5B63067$46517854$tudou",
                                               "\u7B2C\u4E00\u5B63068$46518043$tudou",
                                               "\u7B2C\u4E00\u5B63069$46518277$tudou",
                                               "\u7B2C\u4E00\u5B63070$46518482$tudou",
                                               "\u7B2C\u4E00\u5B63071$46518666$tudou",
                                               "\u7B2C\u4E00\u5B63072$46518946$tudou",
                                               "\u7B2C\u4E00\u5B63073$46519077$tudou",
                                               "\u7B2C\u4E00\u5B63074$46519221$tudou",
                                               "\u7B2C\u4E00\u5B63075$46519321$tudou",
                                               "\u7B2C\u4E00\u5B63076$46519467$tudou",
                                               "\u7B2C\u4E00\u5B63077$46519620$tudou",
                                               "\u7B2C\u4E00\u5B63078$46519771$tudou",
                                               "\u7B2C\u4E00\u5B63079$46519871$tudou",
                                               "\u7B2C\u4E00\u5B63080$46519986$tudou",
                                               "\u7B2C\u4E00\u5B63081$46520132$tudou",
                                               "\u7B2C\u4E00\u5B63082$46520275$tudou",
                                               "\u7B2C\u4E00\u5B63083$46520387$tudou",
                                               "\u7B2C\u4E00\u5B63084$46520500$tudou",
                                               "\u7B2C\u4E00\u5B63085$46520602$tudou",
                                               "\u7B2C\u4E00\u5B63086$46520738$tudou",
                                               "\u7B2C\u4E00\u5B63087$46520859$tudou",
                                               "\u7B2C\u4E00\u5B63088$46521026$tudou",
                                               "\u7B2C\u4E00\u5B63089$46521144$tudou",
                                               "\u7B2C\u4E00\u5B63090$46521260$tudou",
                                               "\u7B2C\u4E00\u5B63091$46521386$tudou",
                                               "\u7B2C\u4E00\u5B63092$46521500$tudou",
                                               "\u7B2C\u4E00\u5B63093$46521620$tudou",
                                               "\u7B2C\u4E00\u5B63094$46521736$tudou",
                                               "\u7B2C\u4E00\u5B63095$46521844$tudou",
                                               "\u7B2C\u4E00\u5B63096$46521949$tudou",
                                               "\u7B2C\u4E00\u5B63097$46522112$tudou",
                                               "\u7B2C\u4E00\u5B63098$46522268$tudou",
                                               "\u7B2C\u4E00\u5B63099$46522348$tudou",
                                               "\u7B2C\u4E00\u5B63100$46522477$tudou",
                                               "\u7B2C\u4E00\u5B63101$46522591$tudou",
                                               "\u7B2C\u4E00\u5B63102$46522779$tudou",
                                               "\u7B2C\u4E00\u5B63103$46522934$tudou",
                                               "\u7B2C\u4E00\u5B63104$46523103$tudou",
                                               "\u7B2C\u4E00\u5B63105$46523237$tudou",
                                               "\u7B2C\u4E00\u5B63106$46523457$tudou",
                                               "\u7B2C\u4E00\u5B63107$46523606$tudou",
                                               "\u7B2C\u4E00\u5B63108$46523720$tudou",
                                               "\u7B2C\u4E00\u5B63109$46523837$tudou",
                                               "\u7B2C\u4E00\u5B63110$46524037$tudou",
                                               "\u7B2C\u4E00\u5B63111$46524174$tudou",
                                               "\u7B2C\u4E00\u5B63112$46524340$tudou",
                                               "\u7B2C\u4E00\u5B63113$46524610$tudou",
                                               "\u7B2C\u4E00\u5B63114$46524789$tudou",
                                               "\u7B2C\u4E00\u5B63115$46524973$tudou",
                                               "\u7B2C\u4E00\u5B63116$46525129$tudou",
                                               "\u7B2C\u4E00\u5B63117$46525929$tudou",
                                               "\u7B2C\u4E00\u5B63118$46526107$tudou",
                                               "\u7B2C\u4E00\u5B63119$46526225$tudou",
                                               "\u7B2C\u4E00\u5B63120$46526340$tudou",
                                               "\u7B2C\u4E00\u5B63121$46526464$tudou",
                                               "\u7B2C\u4E00\u5B63122$46526623$tudou",
                                               "\u7B2C\u4E00\u5B63123$46526772$tudou",
                                               "\u7B2C\u4E00\u5B63124$46526919$tudou",
                                               "\u7B2C\u4E00\u5B63125$46527077$tudou",
                                               "\u7B2C\u4E00\u5B63126$46527191$tudou",
                                               "\u7B2C\u4E00\u5B63127$46527360$tudou",
                                               "\u7B2C\u4E00\u5B63128$46527516$tudou",
                                               "\u7B2C\u4E00\u5B63129$46527654$tudou",
                                               "\u7B2C\u4E00\u5B63130$46527804$tudou",
                                               "\u7B2C\u4E8C\u5B6301$46627567$tudou",
                                               "\u7B2C\u4E8C\u5B6302$46642692$tudou",
                                               "\u7B2C\u4E8C\u5B6303$46642803$tudou",
                                               "\u7B2C\u4E8C\u5B6304$46642939$tudou",
                                               "\u7B2C\u4E8C\u5B6305$46643071$tudou",
                                               "\u7B2C\u4E8C\u5B6306$46643215$tudou",
                                               "\u7B2C\u4E8C\u5B6307$46643310$tudou",
                                               "\u7B2C\u4E8C\u5B6308$46643431$tudou",
                                               "\u7B2C\u4E8C\u5B6309$46653688$tudou",
                                               "\u7B2C\u4E8C\u5B6310$46653791$tudou",
                                               "\u7B2C\u4E8C\u5B6311$46653921$tudou",
                                               "\u7B2C\u4E8C\u5B6312$46653984$tudou",
                                               "\u7B2C\u4E8C\u5B6313$46654087$tudou",
                                               "\u7B2C\u4E8C\u5B6314$46654203$tudou",
                                               "\u7B2C\u4E8C\u5B6315$46654263$tudou",
                                               "\u7B2C\u4E8C\u5B6316$46654335$tudou",
                                               "\u7B2C\u4E8C\u5B6317$46654420$tudou",
                                               "\u7B2C\u4E8C\u5B6318$46654524$tudou",
                                               "\u7B2C\u4E8C\u5B6319$46654620$tudou",
                                               "\u7B2C\u4E8C\u5B6320$46654707$tudou",
                                               "\u7B2C\u4E8C\u5B6321$46654866$tudou",
                                               "\u7B2C\u4E8C\u5B6322$46654957$tudou",
                                               "\u7B2C\u4E8C\u5B6323$46655100$tudou",
                                               "\u7B2C\u4E8C\u5B6324$46655255$tudou",
                                               "\u7B2C\u4E8C\u5B6325$46655417$tudou",
                                               "\u7B2C\u4E8C\u5B6326$46655527$tudou",
                                               "\u7B2C\u4E8C\u5B6327$46655645$tudou",
                                               "\u7B2C\u4E8C\u5B6328$46655780$tudou",
                                               "\u7B2C\u4E8C\u5B6329$46655934$tudou",
                                               "\u7B2C\u4E8C\u5B6330$46656086$tudou",
                                               "\u7B2C\u4E8C\u5B6331$46656167$tudou",
                                               "\u7B2C\u4E8C\u5B6332$46656250$tudou",
                                               "\u7B2C\u4E8C\u5B6333$46656338$tudou",
                                               "\u7B2C\u4E8C\u5B6334$46656462$tudou",
                                               "\u7B2C\u4E8C\u5B6335$46669059$tudou",
                                               "\u7B2C\u4E8C\u5B6336$46669312$tudou",
                                               "\u7B2C\u4E8C\u5B6337$46669600$tudou",
                                               "\u7B2C\u4E8C\u5B6338$46669819$tudou",
                                               "\u7B2C\u4E8C\u5B6339$46670058$tudou",
                                               "\u7B2C\u4E8C\u5B6340$46670253$tudou",
                                               "\u7B2C\u4E8C\u5B6341$46670408$tudou",
                                               "\u7B2C\u4E8C\u5B6342$46670605$tudou",
                                               "\u7B2C\u4E8C\u5B6343$46670881$tudou",
                                               "\u7B2C\u4E8C\u5B6344$46671249$tudou",
                                               "\u7B2C\u4E8C\u5B6345$46671401$tudou",
                                               "\u7B2C\u4E8C\u5B6346$46671542$tudou",
                                               "\u7B2C\u4E8C\u5B6347$46671662$tudou",
                                               "\u7B2C\u4E8C\u5B6348$46671807$tudou",
                                               "\u7B2C\u4E8C\u5B6349$46671912$tudou",
                                               "\u7B2C\u4E8C\u5B6350$46672010$tudou",
                                               "\u7B2C\u4E8C\u5B6351$46672178$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE01$46724867$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE02$46725103$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE03$46725353$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE04$46725566$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE05$46725774$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE06$46725979$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE07$46726189$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE08$46726429$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE09$46726432$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE10$46726671$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE11$46726914$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE12$46726915$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE13$46727139$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE14$46727337$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE15$46727592$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE16$46727776$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE17$46727783$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE18$46728033$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE19$46728250$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE20$46728498$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE21$46728708$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE22$46728927$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE23$46729115$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE24$46729395$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE25$46729614$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE26$46729829$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE27$46730078$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE28$46730299$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE29$46730493$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE30$46730714$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE31$46730913$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE32$46731154$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE33$46731361$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE34$46731609$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE35$46731612$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE36$46792236$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE37$46792574$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE38$46821077$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE39$46821341$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE40$46821346$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE41$46821642$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE42$46821879$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE43$46822152$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE44$46822440$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE45$46822864$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE46$46822871$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE47$46822875$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE48$46823333$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE49$46823609$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE50$46823898$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE51$46824159$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE52$46824453$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE53$46824823$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE54$46824829$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE55$46825033$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE56$46825287$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE57$46825531$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE58$46825534$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE59$46825536$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE60$46826008$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE61$46826012$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE62$46826326$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE63$46826330$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE64$46826587$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE65$46826811$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE66$46827017$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE67$46827278$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE68$46827565$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E09\u5B63\u86C7\u6CBC\u9B3C\u57CE69\u672C\u5B63\u7ED3\u675F$46827696$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE202$46830001$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE203$46830367$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE204$46830712$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE205$46831029$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE212$46832820$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE214$46833428$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE218$46834287$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE219$46834679$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE220$46835023$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE221$46835337$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE222$46836651$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE223$46836931$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE224$46837193$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE225$46837474$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE226$46837720$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE227$46837938$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE228$46838142$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE229$46838147$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE230$46838353$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE231$46838520$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE232$46838738$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE233$46838742$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE234$46838959$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u56DB\u5B63\u4E4B\u8C1C\u6D77\u5F52\u5DE235$46838961$tudou",
                                               "\u7B2C\u4E94\u5B6301$46927950$tudou",
                                               "\u7B2C\u4E94\u5B6302$46928169$tudou",
                                               "\u7B2C\u4E94\u5B6303$46928377$tudou",
                                               "\u7B2C\u4E94\u5B6304$46928559$tudou",
                                               "\u7B2C\u4E94\u5B6305$46928780$tudou",
                                               "\u7B2C\u4E94\u5B6306$46928996$tudou",
                                               "\u7B2C\u4E94\u5B6307$46931187$tudou",
                                               "\u7B2C\u4E94\u5B6308$46929316$tudou",
                                               "\u7B2C\u4E94\u5B6309$46929515$tudou",
                                               "\u7B2C\u4E94\u5B6310$46929691$tudou",
                                               "\u7B2C\u4E94\u5B6311$46929930$tudou",
                                               "\u7B2C\u4E94\u5B6312$46930162$tudou",
                                               "\u7B2C\u4E94\u5B6313$46930376$tudou",
                                               "\u7B2C\u4E94\u5B6314$46931113$tudou",
                                               "\u7B2C\u4E94\u5B6315$47147165$tudou",
                                               "\u7B2C\u4E94\u5B6316$47147513$tudou",
                                               "\u7B2C\u4E94\u5B6317$47147770$tudou",
                                               "\u7B2C\u4E94\u5B6318$47148595$tudou",
                                               "\u7B2C\u4E94\u5B6319$47148268$tudou",
                                               "\u7B2C\u4E94\u5B6320$47455155$tudou",
                                               "\u7B2C\u4E94\u5B6321$47454795$tudou",
                                               "\u7B2C\u4E94\u5B6322$47455217$tudou",
                                               "\u7B2C\u4E94\u5B6323$47672464$tudou",
                                               "\u7B2C\u4E94\u5B6324$47672429$tudou",
                                               "\u7B2C\u4E94\u5B6325$47672812$tudou",
                                               "\u7B2C\u4E94\u5B6326$47767062$tudou",
                                               "\u7B2C\u4E94\u5B6327$47766980$tudou",
                                               "\u7B2C\u4E94\u5B6328$47865676$tudou",
                                               "\u7B2C\u4E94\u5B6329$47865476$tudou",
                                               "\u7B2C\u4E94\u5B6330$48057124$tudou",
                                               "\u7B2C\u4E94\u5B6331$48112626$tudou",
                                               "\u7B2C\u4E94\u5B6332$48198834$tudou",
                                               "\u7B2C\u4E94\u5B6333$48198809$tudou",
                                               "\u7B2C\u4E94\u5B6334$48304295$tudou",
                                               "\u7B2C\u4E94\u5B6335$48421206$tudou",
                                               "\u7B2C\u4E94\u5B6336$48520907$tudou",
                                               "\u7B2C\u4E94\u5B6337$48520906$tudou",
                                               "\u7B2C\u4E94\u5B6338$48571216$tudou",
                                               "\u7B2C\u4E94\u5B6339$48688702$tudou",
                                               "\u7B2C\u4E94\u5B6340$48833625$tudou",
                                               "\u7B2C\u4E94\u5B6341$49083626$tudou",
                                               "\u7B2C\u4E94\u5B6342$49083767$tudou",
                                               "\u7B2C\u4E94\u5B6343$49083791$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63001$71734826$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63002$71734824$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63003$71734830$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63004$71748550$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63005$71749390$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63006$71749482$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63007$71808883$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63008$71808884$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63009$71810009$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63010$71810011$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63011$71811572$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63012$71811573$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63013$71971603$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63014$71971604$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63015$71971606$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63016$71973356$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63017$71974083$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63018$71974358$tudou",
                                               "\u9752\u96EA\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63019$91866903$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63020$72609352$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63021$72609353$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63022$72609354$tudou",
                                               "\u9752\u96EA\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63023$91866904$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63024$72643239$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63025$72643237$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63026$72643238$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63027$72688476$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63028$72688560$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63029$72688701$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63030$72690322$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63032$73295281$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63033$73295257$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63034$73350863$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63035$73350854$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63036$73350855$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63037$73354697$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63038$73354768$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63039$73355317$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63040$73356597$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63041$73356818$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63042$73357404$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63043$73358254$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63044$73358784$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63045$73359264$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63046$73360141$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u516D\u5B63047$73360455$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6301$118589729$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6302$118589730$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6303$118589731$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6304$118603228$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6305$118603851$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6306$118604195$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6307$118604690$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6308$118604759$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6309$118605140$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6310$118722964$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6311$118722965$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6312$118722966$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6313$118723146$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6314$118723147$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6315$118723203$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6316$118730629$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6317$118723405$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6318$118730630$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6319$118723624$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6320$118730631$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6321$118883722$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6322$118883771$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6323$119414540$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6324$119414541$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6325$119508676$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6326$119499957$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6327$119415307$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6328$119510806$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6329$119510807$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6330$119583677$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6331$119583679$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6332$119584928$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6333$119583691$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6334$119670953$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6335$119670952$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6336$119670954$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6337$119672523$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6338$119672524$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6339$119672683$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6340$119672522$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6341$119776478$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6342$119821714$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6343$119821743$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6344$119899485$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6345$120044617$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6346$120069764$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6347$120240268$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6348$120367249$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6349$120396709$tudou",
                                               "\u76D7\u5893\u7B14\u8BB0\u7B2C\u4E03\u5B6350$120518422$tudou",
                                               "\u9752\u96EA\u6545\u4E8B\u76D7\u5893\u7B14\u8BB0\u756A\u5916\u7BC701$121006873$tudou",
                                               "\u9752\u96EA\u6545\u4E8B\u76D7\u5893\u7B14\u8BB0\u756A\u5916\u7BC702$121006874$tudou",
                                               "\u9752\u96EA\u6545\u4E8B\u76D7\u5893\u7B14\u8BB0\u756A\u5916\u7BC703$121006872$tudou",
                                               "\u9752\u96EA\u6545\u4E8B\u76D7\u5893\u7B14\u8BB0\u756A\u5916\u7BC704$121017511$tudou",
                                               "\u9752\u96EA\u6545\u4E8B\u76D7\u5893\u7B14\u8BB0\u756A\u5916\u7BC705$121137007$tudou",
                                               "\u9752\u96EA\u6545\u4E8B\u76D7\u5893\u7B14\u8BB0\u756A\u5916\u7BC706$121247101$tudou",
                                               "\u9752\u96EA\u6545\u4E8B\u76D7\u5893\u7B14\u8BB0\u756A\u5916\u7BC707$121389045$tudou",
                                               "\u9752\u96EA\u6545\u4E8B\u76D7\u5893\u7B14\u8BB0\u756A\u5916\u7BC708$121625649$tudou"
                                           }.Select(
                                               s =>
                                               {
                                                   int seed;
                                                   if(!int.TryParse(Regex.Split(s,"\\$",RegexOptions.Compiled)[1],out seed))
                                                       seed=0;
                                                   return seed;
                                               }).ToList();


        internal Downloader()
        {

        }

        public override string GetContent(string url)
        {
            int i = 0;
            while (i < 3)
            {
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";
                request.ContentType = "text/html, application/xhtml+xml, */*";
                request.KeepAlive = true;
                request.Host = "www.520tingshu.com";
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
                request.ProtocolVersion = Version.Parse("1.1");

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    try
                    {
                        using (StreamReader sr = new StreamReader(response.GetResponseStream(),Encoding.Default,true))
                        {
                            string result = sr.ReadToEnd();
                            return result;
                        }
                        //using (Stream s = response.GetResponseStream())
                        //{
                        //    byte[] content = new byte[response.ContentLength];
                        //    s.Read(content, 0, content.Length);
                        //    string result = Encoding.Default.GetString(content);
                        //}
                    }
                    catch (Exception)
                    {
                    }
                }

                Thread.Sleep(1000);
                i++;
            }

            return String.Empty;
        }

        public int? GetSeedId(int index)
        {
            if (index > this.SEEDLIBRARY.Count)
                return null;
            return this.SEEDLIBRARY[index];
        }

        public override void Download(ISeed seed, CancellationToken token)
        {
            seed.OnStatusChanged(0);

            string url = seed.Url;
            string file = this.GetFileName(seed);
            if (File.Exists(file)) File.Delete(file);

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                wc.Headers[HttpRequestHeader.AcceptLanguage] = "en-US";
                wc.Headers[HttpRequestHeader.Accept] = "text/html, application/xhtml+xml, */*";
                wc.Headers[HttpRequestHeader.Host] = "vr.tudou.com";
                wc.Headers[HttpRequestHeader.UserAgent] =
                    "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";

                try
                {
                    wc.DownloadProgressChanged += (sender, e) =>
                    {
                        int changed = (int)(e.BytesReceived * 100 / e.TotalBytesToReceive);
                        seed.OnStatusChanged(changed);
                    };
                    wc.DownloadFileCompleted += (sender, e) =>
                    {
                        if (e.Error != null)
                        {
                            seed.OnFail();
                        }
                        else if (!e.Cancelled) seed.OnCompleted();
                    };
                    wc.DownloadFileAsync(new Uri(url), file);

                    while (wc.IsBusy)
                    {
                        if (token.IsCancellationRequested)
                        {
                            wc.CancelAsync();
                            break;
                        }

                        Thread.Sleep(1000);
                    }
                }
                catch (Exception)
                {
                    seed.OnFail();
                }
            }
        }
    }
}
