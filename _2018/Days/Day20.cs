using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using _2018.Utils;

namespace _2018.Days
{
    public class Day20 : Day
    {
        private readonly string _paths = 
            "^ESEEEESSWNWSWSEESESENENNNW(SSWENN|)NENWWWWWNWNWWS(SWSEESE(SSWSWNWSSSESWWSSSSEEESWWSESSEENN(WSNE|)NENEEEEESSSEENNNNENEEENNWWS(WWNNNENWW(NENNNNNEEENWNNWWWWWWWSWNNNWNNEES(SENNNWNNWNNNWWNWSWNWNNENNEENNNWSWNNNWWNWWWSWWSEEEE(NWES|)SENESSS(EE|WNWSSSSE(NNESNWSS|)SWWNNNNNWN(E|WWWWNNWSWWSWWWWWNNESEENNEEE(SWWSNEEN|)NWWNWNNWWSESWSE(E|S(WWNNNNWNWWNNENNWNWNNNNWSSWWNENWWWNWSSSWSSENESEEEE(NWWWNWNE(WSESEEWWNWNE|)|SSE(SWSWNNNWSSWSSSWNWSWNNWNNN(NWSWWNENWWNWWNWWNNNESSEEES(W|ENNENNESENNWWNNWWNWNNNWWWSEESSWSSENESESE(N|SSWSWNW(S|NWN(EESEWNWW|)WWNNE(NWWNNE(SEEWWN|)NWWSWWWN(WSWSEEEEESSSE(SSSSWSSWNWSSWNWWWNENEE(SWEN|)ENE(S|NN(ESSNNW|)NWSWNWSWWSW(NNNEN(NNN(WSSSNNNE|)E|ES(SWEN|)EEEN(ESNW|)WW)|SE(SWSSSEEESWSSESSSENNNENEN(WWSWENEE|)EEESSWW(NEWS|)WSWSEENESSEEENNNW(SWSEWNEN|)NNE(SESEE(NWNWESES|)SSSWSW(NNNESNWSSS|)WSWSSWWNENWN(EE|WSSSWSWWNWWNWSW(NNNNNESSE(NNNWWNNE(NWES|)S|EEENE(SSSW(S|WN(W(WW|S)|E))|E))|SESWSSSESESEEEESWWWSSWWNENWNW(N|SSSSSSEEENN(WSWNSENE|)EEENWWNEEESSSSSENENNENNNNWWW(SEESSW(N|SS)|NNNWWSS(ENSW|)WNW(NW(S|WNENNESSE(S|NEEENEEEESWSWNWS(WWW|SSSEENN(WSNE|)EEN(W|NEENENEEEEESSWWN(WWSWSEE(SENEESENNENEESSW(N|SEENNNEEEE(SWWSEESWSWSSSSSSESSENNEESESSWSWWWSWNWSWNWSSWNWWWWWSWWNWNWSWWNNE(NWWW(NENNNENEENNNEE(NWWNWSSSSWNW(NEWS|)SS|ESWWSEESSWNWSSESWWNNWSSW(SEESES(W|EENN(NNEESWSSEEN(W|NEENESENENE(NWWWNWSS(EE|WSWNNWNNESE(NN(E(SEEES(ENN(N(E|NN)|WWW)|W)|NN)|WW(WSSSSE|N))|S))|SSWSSEE(NWES|)S(WWWWNWSW(NNEEES|WS(WNWSW(S|W)|EEE))|EEEN(N|E))))|W(S|W)))|N))|SWSESWSWNNWWSWNWWWN(WWSESWSESESSEEEEENESSWSESWSESSENESENNNNWSW(SEWN|)NNENENENN(W(WS(SW(S|NWWN(WSSWWNENWW(SS|N(WSNE|)E)|N))|E)|NNN)|ESEE(NWES|)SSWW(NEWS|)SESSSWNW(N(N(N|W)|E)|SSESSESSSSSWWNENNNWN(WWWWWSWSESWWWNWWWNNW(NEEEEES(WS(WWNE|ES)|ENN(ESNW|)NNWSWS(WNWSWWNNE(S|EENE(S|N(EESWENWW|)WWSWNW(NN(ESNW|)N|S)))|E))|SSSEESE(ESSWNWWW(NEWS|)SSESEESSENESESWWWWWW(SSSSSSSSEESESSEENESEENENENESSWSESSESWSEENESSWSEENEESENNENWNNENEEEEENWWNWNWWNWNNEENNWSWWWSESSSSSE(SWSWSESSWNW(S|NWNENNNE(NWWWW(NEENWNNWNWSSWNNWSSSES(ENENSWSW|)WWWWWNNNENEE(NWN(ENNW(NNNWS(S|WWN(WWNWESEE|)ENNNESES(W|EEEE(NWWWN(W|NESENNE(NWWSWN|S(ENSW|)S))|ESSESENENN(WSWNSENE|)NNNNESENEENWWNNENEEEENEENNNNESSEESSENNNWWNNWN(WWS(E|WSWSWWSWNWSWSW(SSWSESSENNNEN(W|NESENENES(SWEN|)ENNE(N|SS))|NNENEENESENENWN(EESNWW|)WSWWWSE(WNEEENSWWWSE|)))|EN(ESSSENESENEEEEESESENESENEESSENNNESSSESESWWSSENES(ENNNNENENE(NNNNWNN(WWNWWWSESE(SSEE(NNWSNESS|)ESWWWSSW(SE(ENEN(E|W)|S)|NNWWWSWWWWWNWNENNNNWNWW(NEENWWNEEESEESWS(WNSE|)ESSSW(SSES(EENNNW(S(W|S)|NENEE(N(EEE|NNWSWNNEENWN(WSWWNWN(EE(EE|S)|W(NEWS|)SWS(WNWSWNWWSWNW(NE(EEEEEE|N)|SSES(W|ENE(SE(N|S)|N)))|EESEESSS(S|EE)))|E))|SSW(N|SSENE(SENSWN|)N)))|W)|NN)|SESESSWSSWNNN(WSSSWWWN(WS|EENWW)|N|E)))|N)|EES(SSESWENWNN|)W)|SSWSWSEEE(SESEEN(EN(EES(EENWESWW|)S(SSSS|W(N|W))|W)|W)|N(W|N)))|SSWSWWSWSEEEE(N(NESE(E|N)|WW)|SSSWWNENWWSSSWSSESENN(W|NESSSSWWWNWWSESESSESENNWNEEESWSSESWSWWWSESENEENEN(NNWESS|)EEESSEEEENWWNW(S|NNEEEE(NNWWWS(EE|W)|SS(WNW(S|W)|EEN(EEENWNENN(WSNE|)NESENESEENNNEESWSSEEN(W|NEESENENENWNNWWNWNENESEENNENWWWS(ESNW|)WNWNEEENNESSEENWNENNENESSEESEESWSWSWNN(WSW(NN(N|EE)|SSSW(SSW(SS(WNNWWEESSE|)EEENNN(WSSWENNE|)EENESENNN(WSWWSWNN(SSENEEWWSWNN|)|NESSSEENWNENWNWNNNESSENNNWNWSWSSWNNWW(SEWN|)NNNWWNENNWNENESSEENNNW(SS|NNWWNENEEENNEESSW(SESSWSW(SSESSEENNW(S|N(W|NEES(ENENWNENEENWNWSWWW(NNENNWNWNEENNWSWWWSESSS(E(N|E)|WWWWSS(EENWESWW|)WSWSWWNNNWNNENNWSWWNWWWSWSWSWWSESEEN(NESESSENENE(NNWWNWS(SE(S|E)|W)|SSSE(SSWWWSESSSSSSWWWSEESENESSSENNNNESE(NNWNNNE(SSEWNN|)NENWNW(SWS(SSSW(S(S|E)|NNNN)|E)|NEESEESW(ENWWNWESEESW|))|EESWWSW(SSWWWSWS(E|WWSWWSWNNENWNNWNEENE(ESE(N|SE(ENWESW|)SWWS(E|W(S|NN(WSNE|)(N|E))))|NNWNWNEEENWNNENWNWWSWWSEEE(SWWS(EE|SWSESSE(E|SWWWSW(SSEE(N(N|W)|ESWWSESWSSWNNWSW(SSSEN(N|ESEESENENN(WSW(NNENNSSWSS|)W|ESSEE(SSESSENESSWWWNWN(NWSWS(E|WSS(WNNNEN(ENEEWWSW|)WWSWNW(SWS|NEE)|S))|E)|NENWW(NEWS|)S)))|N))|WNWSWNNEEE(EN(ESNW|)WWNNNEE(SWSEWNEN|)N(ESNW|)NWNNNWWWWWNWNNNEENNENWWSSWWSWWSESSWWWS(ESEEEN(NNN|ESSSEEENWWNEEEESSS(W(NN|WWWWSEEESSWWN(E|W(SSSSES(ENNWNEEENNN(SSSWWWEEENNN|)|WWW)|WNNNW(NEWS|)S)))|E)|WW)|W(W|NNEE(E|NNNNNEENESS(WWSEWNEE|)ENNNWWNWWSS(ENSW|)WNW(SSEWNN|)NWNNNEENNESESSS(WW(S|N(EN|WS))|ENNEENNWNWNWS(SESEWNWN|)WNNENNWNNESEENWNEESSESEENWNNNW(SS|WWWNNESEEEEEENNNEENNNNWWWN(EEEESSSSSENNENWNEN(EEESESSWWN(E|W(NEWS|)SSSSSSSENESSWWWWWSEEESESWSESENENWNEN(NEEESENNWWWNW(NWNENWW(NEEESESEEENWNNENESESW(W|SEEEEENNNENESSSSENENNESEESESESSENNNESESWSSWSEEENWNEESENNWWNEENNNESESWSSSESENNWNEESE(SWSESWWWSESWSESSSSWNNNWNNNN(WSSWN(N|WSSEESSSSSWSSEEN(N(NNN|ESENESE(NNWNENNW(NNNESS|S)|SSSSSSSWNNWSSSEESSSSSWNWSWWNENWWSWWNNWWNWNENWNWSSSWSWNNW(NNENNNWS(S|WNNEENENESENENESE(SSESSEEE(ENE(NWW(NEE|SWNWS)|S)|SSSSSES(SENNSSWN|)WWNNNWSSSW(NNNNEENWWWNWWWS(EES(SE(SSWNSENN|)N|W)|WNNN(WSSSSS(W|S)|EEE(NWES|)S(E|WW)))|S))|NNENWWNWNWSWSES(WS(WWWS(E|WWS(EE|WWNENEENNENENE(ENNWSWWSWWNWWWSWWWWWWSEEESWWWWSEEESENENEESS(W(N|WSEESWWWSWWWSESSWS(WWNEN(E|WWWWWSS(ENESE(S|N)|WNNWSSWS(SWWWWNENNNWSWNNWNNNESSEENWNNNNNWWSSWNN(NEENNNN(WWWSSENESSW(ENNWSWENESSW|)|NES(ENNN(WSWNSENE|)ESSSENNNNN(SSSSSWENNNNN|)|SSSSEESWSSESEESSWNW(WNSE|)SSS(WW|SES(WSESWENWNE|)ENNN(WSNE|)EES(EENNNENNWSWSWW(SEESNWWN|)NEN(WWNW(NNNWWEESSS|)(S|W)|ENEEESE(SWSESWWSSENESENNENE(SENE|NWWS)|NNWWWNNNESSEEE(N(ENWNENWNN(ENESENESEESWWSWSW(SSENESENN(W|EEENESENNESENNEE(NWNWNWSWSWS(EENEWSWW|)WW(N(WW|NNN|E)|S(SWWEEN|)EE)|SESWW(N|S(EEESEE(NWES|)S(SESENSWNWN|)WW|SWW(NEWS|)(S|WWWWWS)))))|NN(E|W))|WSWWN(WNSE|)E)|WW)|S)))|W))))|WWSWWS(WNNE(NWWSSNNEES|)E|SEESEN(N(WW|N)|E(EENNSSWW|)SSWWSW(SWSS(WNNSSE|)ESENENN(WSWENE|)E(NWES|)SESWSEE(E|SSSESWSWSWNN(WSSSES(EEESES(WSNE|)ENENEN(WWSWNN(E|W(WS(W|E)|NENNEE))|ESE(NN|EEE(N|S(WWS(WNWWEESE|)SSESW(W|SESWS(E|WW))|E))))|WWNWNE)|NNE(NWW(WW|N)|SS)))|NN))))|E)))|SENENNEN(W|ENESENESEE(NWES|)SWWS(WWN(WSWSESE(N|SS(ENSW|)WNWSWN(NEWS|)WWSS(SE(ENWN|SWS)|W))|E)|E))))|ENEENN(WNWS(SEWN|)WWW|E))|SSSW(N|W))))|E)|E(E|N))))|SSS(SWEN|)EEESESESESENN(WNWN(WNWESE|)E|ESE(N|SWSWSSWSSSWSWWNNE(S|NWNW(NENN(WSNE|)E(ESWSSE(NENSWS|)SS|N)|WSWWSESSSWSW(WWWSWNW(S|NNWNEESE(NN|S(W|ENEE(SW|EN))))|SESSSESWS(W|SSSENEESESESESSWSSESSENNN(W|NENNESESEEESWSWWWN(N|WSSEEESSSWWNENWWSSWWN(E|WNWWNWSSSSSWNWNNE(NNWSWSSWWSEESWSEE(N|SSSWSEESENNESEESENNWNEENENNESENN(WWWSWNWNWW(NEWS|)SSE(N|SE(N|E(E|SWWS(S|WNNWSW(N|S(E|S))))))|EEENWNNEN(W|ESE(NNNW(SWEN|)NNWNENE(SSS|NWWSWSSWNNNENNNENESE(NNWWNNWWSSS(ENNSSW|)SWSWWSWSW(SE(ENEE(S(S|W)|N(ENSW|)W)|SS)|WNNW(S|NEEE(SWSNEN|)NWNWWS(WSSWWNENNENENENEEENENNNEESEESESS(SE(SWEN|)NNNNNNNWNENNWWS(SSWWS(EES(E(S|N)|W)|WNNW(SWS(SSWS(E|SWWN(E|WSWS(WN(NENNW(NENSWS|)S|WSSSENE(WSWNNNSSSENE|))|E)))|E)|NEEE(SWEN|)NNEN(WWSSW|EESW)))|E)|WWWNN(ESEWNW|)W(SSSSSSWNW(SSEWNN|)WNEN(WWSWENEE|)E(S|N)|N))|E)))|SWWSEE))|SSSW(NNWESS|)SESSWWN(E|NWWSESSSSWSWNWNN(ESENNWW(EESSWNSENNWW|)|WSSSESEEESWWSWWWWWNWSSSWNNNWN(ENESEESEN(ESEWNW|)NNN|WSWWWSWNWNWSWSESSEN(N|EEESEEE(SEESENESSSENESESEESESESWSSWWSESESEENE(NNWSW(S|W|NNEENNW(S|NENNNNNWSSWNNWNNN(ESSENE(S|NWNWNN(ENE(NNWSW(S|N)|SS(S|W))|W))|WSWWSSEE(NWES|)SESWW(WN(E|WNW(S|NENWWWSE(WNEEESNWWWSE|)))|SEEEESW(W|S)))))|SSSSSWSSWSESE(SSSWNNWWSESWWWS(WNNNENWNNNESSES(SSWENN|)E(E|NNWNNENWNEN(WWSWSS(ENSW|)WNNNWWSSE(SSWSWWWNENWWWSSWSESEESESENESSS(ENNE(SS|NWNWWN(EEE(N(W|N)|S)|W(WWNNSSEE|)S))|WNWWS(E|WNN(NWSWNWN(WSSWSES(WWWWNEENNENWNNWSWWNNE(NNWWWNENENNNWSWWWNEENNWNENNNNEEESSEEESEENNW(NENNWWS(E|WNWN(EENESEESE(N|EESESSSESWWSEESWWSESSWNWWSESESESS(WWN(WSWNNWWNWNWWSSS(W(NNNNEEENNEN(WWSSWWNENN(WWW(NNESNWSS|)SE(SSWNSENN|)E|E)|ESSWSSEE(S(ESEWNW|)W|N(NNNEE(SWSEWNEN|)NNNN(ESSNNW|)NWWSSE(SSWNSENN|)N|W)))|W)|SSEENWNE(NWNSES|)ESSE(ESWWSW(NWW|SEEN)|NN))|E)|E(S(E|S)|NNN(W|NNESSEENE(NWNW(NNW(SSWENN|)NEESES(S|W|EENWN(E|NW(WNWSWNWNN(SSESENSWNWNN|)|S)))|SS)|ES(WSNE|)E)))))|WWNENWNNWWNNENNNNWNENESSEENEE(SSW(N|WWWSEESWS(EEESEN(EESWENWW|)NN(NNES|WWSE)|SW(W|NN)))|NWWW(S|NEENNENE(NNNWSSWNWSS(SWS(WNWNEENNW(WS(E|WSSWNW(NNE(S|E)|SSESWWNW(NENSWS|)SWWSWSSWSESWSSESSSESSSSESSEENWNNNEESENNN(EESWSSE(SWS(WSSSSWNN(NNN(WNSE|)E|WSSSSWWSESWSWWNWNENNNES(EENWNENWWSWNNWSSWSESSWNWNWWNEENWNNNNESSSE(S|NNEE(NWWNEENWNWWWNEEN(ESNW|)NWSWWWWNEENE(S|EENWNWNNWSWSS(E(E|N)|WNNW(SWSSSSE(NN(EE|N)|SESWWSSWNNWWWNWNN(EEENWN(WSWNSENE|)EE(NWES|)SSSWW(SEEEWWWN|)W|WSWSWWWNWSWSEEEEESSSENNE(NWNSES|)SSSSSWNNWSSWNNWSWWSSSWWWWSESWSESEEEENWWNW(NEESEENWNEESSSEESWSEESESESEENWNEEENNNENE(NWNWSWWSSWWNWSS(EEEE(NNEWSS|)SWWW(W|S)|WNNNWNN(W(WWWNE|SSSE)|ESENENNENN(ESSSWSS(ENE(ENESE(ENWNNWSWW(NNE(EENWN(WSNE|)ENE(NWES|)SES(WSSS|ENE)|S)|S)|S)|S)|WS(ESNW|)W)|NWN(WSSS(ENSW|)SS|E))))|SENESSSEN(ESSWWWNWSW(NNEEWWSS|)SWWSESESWSESENENWNN(NEESS(WNSE|)EESSEEEENWWWNNEN(W(WSWNSENE|)N|ESS(ENEESWSESWSEESWWWWN(E|WSWSWWNEN(E|WW(NNESNWSS|)WSSSSEESENN(WWNNSSEE|)ESSENNNESSENEN(ESESENEE(SSW(N|WSS(EEEN(WW|NEENNWSWN(SENESSNNWSWN|))|WWWWNN(ENESESWW(EENWNWESESWW|)|WSSWNWSWNWSWWWWWWWNENENESS(W|EN(ESENSWNW|)NNNWWWWSWNWSWWNENEENEE(NWWNN(ESENSWNW|)WSSSWWWNNESENNWWNE(NWNWWSWWNWWWWSSWWSWWWNNESENEENWNE(EEEEEE(ENWNSESW|)S|NWNNWWSESWSWWS(EEENSWWW|)WSSWSWSSESWSESESWWWSSWWNWWNNWSSSWWNNNE(SS|NWNENENNWWS(E|WS(E|SWWWWSSSESWSSES(ENN(ENWNENN(WSWNSENE|)ESSSSSEEES(WWWWNSEEEE|)EEEN(WWNEWSEE|)EEEES(EENENNW(SW(S|WN(WSNE|)E)|NEESSSS(W|ENESEEENEENWWNWNNEENWNEENNWNNW(NEESSENESSSW(SSWSEEESSEE(SEEN(W|ESE(EN(ENEEWWSW|)W|SSWNWSWWN(E|WSWNN(E|WW(SS(ENSW|)WW|NN(ESNW|)WW(SEWN|)W)))))|NWNN(E(E|S)|WN(WSNE|)ENNW(NNE(N|S)|S)))|N)|SSS(WSSWSWNWS(WWNENEENWWN(EEE(ENWESW|)SS|WSSWS(WNNN(W|NE(SS|NN))|S))|SEESWWSEESEN(SWNWWNSEESEN|))|E))))|WWW)|W)|WWWNENNNWWSS(ENSW|)WWNNWSSWSES(WWWWNENE(S|NWWSWSSWNWSWNNWSWNWNEENWWWWWSESSS(WNWWW(SEEWWN|)NNNENWNEENNNENNWNNWSW(SSSE(SSWNSENN|)N(E|N)|NNENNW(S|NENESSSSENENENNNWSW(S(S|E)|NWNWNW(NNNEESWSEES(W|ES(EEEEESWWWSESEEN(W|ESSSWWWSWW(SSESSENEENNW(W(SEWN|)W|NEESSSEESSSWSEESENEESWSE(S(S|WWWN(WSSWNNN(WWNWNEE(S|NENWWWS(E|WWSESSWS(EE(N|ESSE(SEWN|)NN)|WWNEN(E|WW(SSWSSS(WNSE|)E|NENE(NWN(E|NN(WSNE|)N)|S))))))|E)|E))|EEE(SS|ENWNENNNEEESWS(EENENWNEENNWWWNWNNNNNNWNENESSESENNWNNENNWWS(WWS(W(NNENN(WSWENE|)EES(WSNE|)ENESEESWSSS(SSSSSSESWWNWW(SESEEEENNESSSSENNNNNNNESENEEENENNNNNWWNWWSESWSSWWNNWSSWNW(SSEEES(WWWSSEE(SWW|NW)|ENEEE(SWWEEN|)ENWN(WS|ENW))|NENNNEESS(WNSE|)E(SS|NNENNEEEESWS(WWNEWSEE|)SEESESSSSW(NNN|SSWSSWWSWSWNW(NEENEE|SSESWSS(SSEESESSE(NNNWNNNW(NEN(NESESE(NNE(EENWNENENNNNNENWWNW(NEENWWW(W(NNNNNEENE(SSWWSSSEENN(WSNE|)ESES(W|SEEEE(SWSEESWWSEE(EENNW(N(EESE(SWEN|)N|W)|S)|SSWWWN(EE|NNNW(SSSSW(SE(SWSEWNEN|)EEEE|N)|N(WSWENE|)E)))|NWNW(SWNSEN|)NEENEE(NWES|)S(WSWENE|)EEE(SWEN|)E))|E(NWNNWNWWWWWWNNNESSENNEEE(SSWWNE|NWNNWSSWNNNENENEENNE(ENNWSWWWNNNWSWWNENEENN(WWNWNWSSES(WS(SWSSSEEEN(WWNSEE|)ESSS(ENESNWSW|)SWSWWNENENWWWWNWSWSWNNENEENNNWNW(NEN(W|EE(SWSES|NW))|SSWWWW(NENWESWS|)WSSWWSESSWWSSWSWNWN(WWSSE(N|ESESWSSSWNNNW(NEWS|)SSW(SESESS(ENNESENNWNEENEENWNEESESESEESENNEEE(SWSSW(WSWSWWSSESSEEESWSEE(NNESENNESSES(ENNWNNNN(EESWSEE(ENWN|SWW)|WSWS(WNN(E|N(WSSSS(WWNW(N(EE(S|N(N|W))|W)|S)|E)|NN))|E))|WWW)|SWWWNNWWNWSS(WWNENWW(NW(SWNWESEN|)NNNNEESSW(SEES(E(NNWNENENWN(EES(S|E)|WN(E|WWSS(WNSE|)E(ESNW|)N|N))|E)|W)|N)|S)|ESWSESEN(NN|EEEE)))|NN)|EENNWSWNWWS(E|WWNNNEEENN(WSWWWSWWWN(NENN(WSNE|)NNESE(SWSS(EN|SW)|EN(EENSWW|)W)|WWSES(WWSWS(SWEN|)E|EEEESS(SE|WNW)))|E(SSS(WWWSNEEE|)EE|E))))|WWN(NWWS(WNWSWWNNE(S|NEN(NNWSWNWSSE(E|S)|ESEESWWW(EEENWWEESWWW|)))|SE(SES(SSENSWNN|)W|N))|E))|N))|NESENNNE(SEWN|)NWW(W|NEEEEN)))|E)|EE)|EESE(NN|EE(E|SWSWWN(E|W(N|SSSENEEE(SSENEWSWNN|)N)))))|SSSWWS(ESWSEE(SWSEWNEN|)NEN(W|N)|W)))|EE))|W)|S)|SSESWSSS(ENNSSW|)W(S(SW(N|S(W(S|W)|E))|E)|N))|S)|SWSW(NN|S))|W)|SW(SEWN|)N)|S(WS(WNNW(SS|WN(NESE|WS))|S)|E))|WWWW(SWSSW|NENW))))))|NEENWWN(SEESWWEENWWN|))|W)|WSE(SSWNWSWNW(N|WSWSEE(N|SSWNWSSSESESSSENNNNWN(EENNN(WSSNNE|)ESESE(NNWESS|)SSWNW(N|SWSSEESES(EN(NWNWWEESES|)E(EE|S)|WWW(NEWS|)SESWS(E|SSWNNNWSWWWNENNW(S|NWNEE(NWWNW(SSS|NNESE(NNWNNE(S|N(WWSS(WNNWWWNEENNWSWWWW(SESENSWNWN|)WNENNWSWWNEN(EEESESWSEENNN(ESES(E(NNWNSESS|)SSES(ENSW|)W|W)|W)|NNWW(NEWS|)SESWSS)|S)|E))|S))|SE(N|SS(EE|S)))))))|W)))|E))|EE)|E)|SW(SESWENWN|)NN))))|NEN(EEE|N)))|W))|SS(S|E)))))|EE(NWNNSSES|)EE))|EEENWW))))))|E)|ES(WW|EE))))))|NWW(W|NNESENN(E|WWWNNNE(EN(E|WWWW(NENNE(E|NN)|W))|SS))))|W)))|W))|W)|N))|S)))|NENN(WSNE|)ES(ENEES(W|ENE(SSWS(WNSE|)S|EN(EE|N)))|S))))|S(ESNW|)W))|SSS))|EEE(NWES|)ESEESWW(EENWWNSEESWW|))|N)|WWS(WNW(NENWNNEEESSS(WNNWESSE|)ENNNNWWNNWNEESE(NN(ENNESE|WWWWSSW(N|SS(EENWESWW|)S(SSS|W)))|S(ESSNNW|)W)|SSS)|E))))|NNNEEEES(WSWNWS|EESS(EEEEE(SEWN|)NWNEN(WN(NW(SSSWW(SEEWWN|)NWN(W(S|W)|EE(NWES|)S)|N)|E)|E)|S)))|E)|E)|S)))))|S)|S)|EN(EESWENWW|)N)|E)|E)))|N)|ESE(SWSSWENNEN|)N))|EEEEE)|NNN))|NNWW(SEWN|)W)))))))))|S))|EE))))))))|W)))|W))|N)|NNNNWNN(ESNW|)WWS(ES(SSENSWNN|)W|WWWSWSW(SEENSWWN|)WNWNWSS(E|WNWNN(E(S|EEEES(ENEEEWWWSW|)(S|W))|WS(WNWW(S(SS|E)|WWWSWWSESSWNW(S|NNW(S|WN(WSWSWSW(SESNWN|)NW(S|NN(ESENEWSWNW|)W)|EEEE))))|S))|S))))|SS)|S)|WW))|W)|WWSWNWSSSWWNNN(ESSNNW|)WWSESSWWN(NNWSWWSEESSSEEEN(WW|ESS(WWWWSSESENN(ESSSWWSWNNWSSWNWWNNENENWNNN(ESE(S(W|SSSWSW)|N)|NWNWWSWNN(EEEE(EE|S)|WWWWSWNWSSWSEEEN(W|ENESE(SWWSSEEN(W|EESSSWWSESWWNWSWSWSESENESEENESSENESEN(NWWNW(NNE(NNE(S|NNNW(N|SS|WW))|S)|WW(WW|S))|EEESE(S(E|WWNWSSESWWSWNNENWWWWWWN(EE|WWSWNNWWSWSWNWNNWNEENNENNEENN(WSWNWNWSSWSWWNWNEE(S|ENNNN(WWSSS(ENNSSW|)WNWNWN(EESNWW|)WSWSEESWWWSWSWNNWSWNW(WWNNESEEEEE(ENN(E|WSWWN(WSWNWWWSS(NNEEESNWWWSS|)|E))|S)|SSEESSENESS(EENENEE(ESESES(ENN(W|E(SS|EN(W|N)))|SSWNW(SSSEEN(ESSSE(N|SENEEE(ESSSWSW(SEESW(W|SEEENNNW(NEENNN(EEEESSSSESWWWWSSSE(EEEEE(SEWN|)NNNNW(N(W|ENEE(SWSE|ENNWS))|SSWS(WWN(WSNE|)E|E))|SWW(N(WWSEWNEE|)NNNNENESENNWWNEE(WWSEESNWWNEE|)|SS))|W(WSSENSWNNE|)N)|SS|W))|WNENN(ESNW|)WSWWWW(S(ESENES|W)|N(NNWESS|)E))|N(W|N)))|W)|N(NWSWW(NENEWSWS|)WW|E)))|NWN(ENSW|)WWS(WS(S|E)|E))|WWSWWSSW(S(ESENNENW(ESWSSWENNENW|)|W)|N)))|EEES(ENSW|)S(WNWSNESE|)S))|EESWSSWWSSE(EN(ENEN(E(EENSWW|)S|W)|W)|SSWW(WSEWNE|)N(N|E)))))|N)))|N))))|W)|EESEENNEE(SWSEWNEN|)NNEE(SWEN|)NWWW(N|WWSES(ENSW|)WWWSEES(NWWNEEWWSEES|))))|E)))))))|S))|N))|N)))|N))|NN))|W))|SE(SWSEWNEN|)E)|W)))|NN(E|NW(W|SS)))|N)))|NN)|NN))|E))|W)))))))|W)))))|S)|WWS(E|SWNWN(WWSESESESSWS(WWNEN(WN(E|N)|E)|ES(W|S(EEN(EE(SWSNEN|)E|W)|S)))|E)))|SSW(WSEEWWNE|)N)|SSEE(NWES|)SWWSES)|SS))|EN(WN|ESE))|NN(EESWENWW|)N)|N))|N)))|EEENWNEESS(NNWWSEWNEESS|)))|S)|NNWNWWSS(E(E|N)|WWWWW(N(EENN(N|ESE(NEN|SW))|WWWNWWNNW(SWSES(EE|WSWW(SEWN|)N(ENW|WS))|NNESE(SSEWNN|)N))|S))))|N)|E)))))|S)))))|NWWNW(SWWEEN|)NENNNE)|EEEN(WW|E))))|N)|EE)|S))))|ESSEE(SWEN|)NN(WS|ES))|E|N))|S))))|W)|SWSESSWNWWSWSWS(WW(S|WWWNEEEENWNNEES(S|W|EN(EE|NWNWSW(W(SSSWWEENNN|)W|N))))|EEEEE(SSS|N(WW(W|N)|E))))|E)|E)|E)$";

        private readonly HashSet<Point> _doors = new HashSet<Point>();

        private HashSet<Point> CalculateDoors(LinkedListNode<char> pathPoint, Point pos)
        {
            var trackedPoints = new HashSet<Point>
            {
                pos
            };
            
            while (pathPoint != null)
            {
                var dx = 0;
                var dy = 0;
                
                switch (pathPoint.Value)
                {
                    case 'N':
                        dy = -1;
                        break;
                    case 'E':
                        dx = 1;
                        break;
                    case 'S':
                        dy = 1;
                        break;
                    case 'W':
                        dx = -1;
                        break;
                    case '^':
                        break;
                    case '$':
                        break;
                    case '(':
                        var (groups, groupFinishPoint) = GetPathOptions(pathPoint);

                        var newTrackedPoints = new HashSet<Point>();

                        foreach (var group in groups)
                        {
                            foreach (var trackPoint in trackedPoints)
                            {
                                foreach (var newTrackPoint in this.CalculateDoors(group.First, trackPoint))
                                {
                                    newTrackedPoints.Add(newTrackPoint);
                                }
                            }
                        }

                        trackedPoints = newTrackedPoints;

                        pathPoint = groupFinishPoint;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"{pathPoint.Value} is not one of NESW");
                }

                if (dx != 0 || dy != 0)
                {
                    var newTrackedPoints = new HashSet<Point>();
                    
                    foreach (var point in trackedPoints)
                    {
                        this._doors.Add(new Point(point.X + dx, point.Y + dy));
                        newTrackedPoints.Add(new Point(point.X + dx * 2, point.Y + dy * 2));
                    }

                    trackedPoints = newTrackedPoints;
                }

                pathPoint = pathPoint.Next;
            }

            return trackedPoints;
        }

        private static (IEnumerable<LinkedList<char>>, LinkedListNode<char>) GetPathOptions(LinkedListNode<char> pathPoint)
        {
            var numBrackets = 1;
            var prevPoint = pathPoint;
            pathPoint = pathPoint.Next;
            var pathOptions = new HashSet<LinkedList<char>>();
            var currentPath = new LinkedList<char>();

            while (numBrackets > 0 && pathPoint != null)
            {
                switch (pathPoint.Value)
                {
                    case '|':
                        if (numBrackets == 1)
                        {
                            pathOptions.Add(currentPath);
                            currentPath = new LinkedList<char>();
                        }
                        else
                        {
                            currentPath.AddLast('|');
                        }
                        break;
                    case ')':
                        numBrackets--;
                        if (numBrackets != 0)
                        {
                            currentPath.AddLast(')');
                        }
                        break;
                    case '(':
                        numBrackets++;
                        currentPath.AddLast('(');
                        break;
                    default:
                        currentPath.AddLast(pathPoint.Value);
                        break;
                }

                prevPoint = pathPoint;
                pathPoint = pathPoint.Next;
            }

            pathOptions.Add(currentPath);

            return (pathOptions, prevPoint);
        }

        private void PrintDoors()
        {
            var minX = int.MaxValue;
            var minY = int.MaxValue;
            var maxX = int.MinValue;
            var maxY = int.MinValue;

            foreach (var door in this._doors)
            {
                minX = Math.Min(minX, door.X);
                maxX = Math.Max(maxX, door.X);
                minY = Math.Min(minY, door.Y);
                maxY = Math.Max(maxY, door.Y);
            }

            var map = new StringBuilder();

            map.AppendLine(new string('#', maxX - minX + 3));

            for (var y = minY; y <= maxY; y++)
            {
                var line = new StringBuilder();

                line.Append('#');

                for (var x = minX; x <= maxX; x++)
                {
                    if (x == 0 && y == 0)
                    {
                        line.Append('X');
                    } else if (x % 2 == 0 && y % 2 == 0)
                    {
                        line.Append('.');
                    }
                    else if (this._doors.Contains(new Point(x, y)))
                    {
                        line.Append(x % 2 == 0 ? '-' : '|');
                    }
                    else
                    {
                        line.Append('#');
                    }
                }

                line.Append('#');

                map.AppendLine(line.ToString());
            }

            map.AppendLine(new string('#', maxX - minX + 3));
            
            ConsoleUtils.WriteColouredLine(map.ToString(), ConsoleColor.DarkGreen);
        }

        private (int longestPath, int numThousandPaths) GetLongestPath(Point start)
        {
            var queue = new Queue<Point>();
            var seenPoints = new Dictionary<Point, int> {{start, 0}};

            queue.Enqueue(start);

            while (queue.TryDequeue(out var consideredPoint))
            {
                var consideredDistance = seenPoints[consideredPoint];

                for (var dx = -1; dx <= 1; dx += 2)
                {
                    var doorHopeful = new Point(consideredPoint.X + dx, consideredPoint.Y);

                    if (!this._doors.Contains(doorHopeful)) continue;

                    var nextPoint = new Point(doorHopeful.X + dx, doorHopeful.Y);

                    if (seenPoints.ContainsKey(nextPoint)) continue;
                    
                    seenPoints.Add(nextPoint, consideredDistance + 1);
                    queue.Enqueue(nextPoint);
                }
                
                for (var dy = -1; dy <= 1; dy += 2)
                {
                    var doorHopeful = new Point(consideredPoint.X, consideredPoint.Y + dy);

                    if (!this._doors.Contains(doorHopeful)) continue;

                    var nextPoint = new Point(doorHopeful.X, doorHopeful.Y + dy);
                    
                    if (seenPoints.ContainsKey(nextPoint)) continue;
                    
                    seenPoints.Add(nextPoint, consideredDistance + 1);
                    queue.Enqueue(nextPoint);
                }
            }

            return (seenPoints.Values.Max(), seenPoints.Values.Count(d => d >= 1000));
        }
        
        protected override void DoPart1()
        {
            var paths = new LinkedList<char>(this._paths);

            this.CalculateDoors(paths.First, new Point(0, 0));
            
            this.PrintDoors();

            var (longestPath, _) = this.GetLongestPath(new Point(0, 0));
            
            ConsoleUtils.WriteColouredLine($"Got longest path of length {longestPath}", ConsoleColor.Cyan);
        }

        protected override void DoPart2()
        {
            var (_, numThousandPaths) = this.GetLongestPath(new Point(0, 0));
            
            ConsoleUtils.WriteColouredLine($"Got {numThousandPaths} paths of at least 1000 doors", ConsoleColor.Cyan);
        }
    }
}