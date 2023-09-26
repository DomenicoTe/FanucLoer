using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace FanucLogIc
{
    public struct SystemInfoEX
    {
        public short max_axis;
        public short max_spdl;
        public short max_path;
        public short max_mchn;
        public short ctrl_axis;
        public short ctrl_srvo;
        public short ctrl_spdl;
        public short ctrl_path;
        public short ctrl_mchn;
        public string system;
        public short group;
        public short attrib;
        public short path_ctrl_axis;
        public short path_ctrl_srvo;
        public short path_ctrl_spdl;
        public short mchn_no;
    }
    public struct SystemInfo
    {
        public string addinfo;
        public string cnc_type;
        public string mt_type;
        public string series;
        public string version;
    }
    public struct StatInfo
    {
        public short tmmode;
        public short aut;
        public short run;
        public short motion;
        public short mstb;
        public short emergency;
        public short alarm;
        public short edit;
    }
    public struct StatInfo2
    {
        public short warning;
        public short o3dchk;
        public short ext_opt;
        public short restart;
    }
    public struct Diag_EXT
    {

    }
    public partial class Functions : Form
    {

        short ret;
        ushort h;
        int timeout = 5;
        

        public Functions()
        {
            InitializeComponent();
        }
        #region Fanuc
        private string F_GetActivePath()
        {

            ret = Focas1.cnc_getpath(h, out short active_path, out short max_path);
            return active_path.ToString();
        }
        private string F_GetMaxPath()
        {
            ret = Focas1.cnc_getpath(h, out short active_path, out short max_path);
            return max_path.ToString();
        }
        private string F_SetPath(short wanted_path = 0)
        {
            //Set il path da usare ottenuto dalla funzione precente, Default = 0
            ret = Focas1.cnc_setpath(h, wanted_path);
            return wanted_path.ToString();
        }
        private SystemInfo F_GetSysInfo()
        {
            /*cnc_sysinfo*/
            Focas1.ODBSYS sysinfo = new Focas1.ODBSYS();
            ret = Focas1.cnc_sysinfo(h, sysinfo);
            SystemInfo message = new SystemInfo
            {
                addinfo = "",
                cnc_type = "",
                mt_type = "",
                series = "",
                version = "",
            };
            if (ret == 0)
            {
                message.addinfo = message.addinfo = sysinfo.addinfo.ToString();
                message.cnc_type = message.cnc_type = sysinfo.cnc_type[0] + sysinfo.cnc_type[1] + "";
                message.mt_type = message.mt_type = sysinfo.mt_type[0] + sysinfo.mt_type[1] + "";
                message.series = message.series = sysinfo.series[0] + sysinfo.series[1] + sysinfo.series[2] + sysinfo.series[3] + "";
                message.version = message.version = sysinfo.version[0] + sysinfo.version[1] + sysinfo.version[2] + sysinfo.version[3] + "";
            }
            return message;
        }
        private SystemInfoEX F_GetSysInfoEx()
        {
            /*cnc_sysinfo_ex*/
            Focas1.ODBSYSEX sysinfoex = new Focas1.ODBSYSEX();
            ret = Focas1.cnc_sysinfo_ex(h, sysinfoex);
            SystemInfoEX message = new SystemInfoEX
            {
                max_axis = 0,
                max_spdl = 0,
                max_path = 0,
                max_mchn = 0,
                ctrl_axis = 0,
                ctrl_srvo = 0,
                ctrl_spdl = 0,
                ctrl_path = 0,
                ctrl_mchn = 0,
                group = 0,
                attrib = 0,
                path_ctrl_axis = 0,
                path_ctrl_srvo = 0,
                path_ctrl_spdl = 0,
                mchn_no = 0,
            };
            if (ret == 0)
            {
                message.max_axis = sysinfoex.max_axis;
                message.max_spdl = sysinfoex.max_spdl;
                message.max_path = sysinfoex.max_path;
                message.max_mchn = sysinfoex.max_mchn;
                message.ctrl_axis = sysinfoex.ctrl_axis;
                message.ctrl_srvo = sysinfoex.ctrl_srvo;
                message.ctrl_spdl = sysinfoex.ctrl_spdl;
                message.ctrl_path = sysinfoex.ctrl_path;
                message.ctrl_mchn = sysinfoex.ctrl_mchn;
                message.system = Convert.ToString(sysinfoex.path.data1.system, 16).ToUpper();
                message.group = sysinfoex.path.data1.group;
                message.attrib = sysinfoex.path.data1.attrib;
                message.path_ctrl_axis = sysinfoex.path.data1.ctrl_axis;
                message.path_ctrl_srvo = sysinfoex.path.data1.ctrl_srvo;
                message.path_ctrl_spdl = sysinfoex.path.data1.ctrl_spdl;
                message.mchn_no = sysinfoex.path.data1.mchn_no;
            }
            return message;
        }
        private string F_GetAbsAxis(short crtlAxis)
        {
            string message = String.Empty;
            short n_axis = -1;
            short ldati = 4 + 4 * Focas1.MAX_AXIS; //aggiornare il FWLIB32.CS a seconda del tipo di controllo, per avere il giusto numero di MAX_AXIS
            Focas1.ODBAXIS coord_abs = new Focas1.ODBAXIS();
            ret = Focas1.cnc_absolute(h, n_axis, ldati, coord_abs);

            for (int i = 0; i < crtlAxis; i++)
                message += "asse: " + i + "= " + coord_abs.data[i].ToString() + "\n";

            return message;
        }
        private string F_GetCoord(short crtlAxis)
        {
            string message = String.Empty;
            short n_axis = -1;
            short ldati = 4 + 4 * Focas1.MAX_AXIS; //aggiornare il FWLIB32.CS a seconda del tipo di controllo, per avere il giusto numero di MAX_AXIS
            Focas1.ODBAXIS coord_machine = new Focas1.ODBAXIS();
            ret = Focas1.cnc_absolute(h, n_axis, ldati, coord_machine);

            for (int i = 0; i < crtlAxis; i++)
                message += "asse: " + i + "= " + coord_machine.data[i].ToString() + "\n";
            return message;
        }
        private string F_GetCoord1(short crtlAxis)
        {
            Focas1.POSELMALL posnew = new Focas1.POSELMALL();
            short n_axis = Focas1.MAX_AXIS;
            string message = String.Empty;
            byte[] bytes = new byte[Marshal.SizeOf(posnew) * Focas1.MAX_AXIS];

            IntPtr ptrWork = Marshal.AllocCoTaskMem(Marshal.SizeOf(posnew));
            ret = Focas1.cnc_rdposition(h, 0, ref n_axis, bytes);
            for (int i = 0; i < crtlAxis; i++)
            {
                Marshal.Copy(bytes, i * Marshal.SizeOf(posnew), ptrWork, Marshal.SizeOf(posnew));
                Marshal.PtrToStructure(ptrWork, posnew);
                message += posnew.abs.name.ToString() + " = " + posnew.abs.data / Math.Pow(10, posnew.abs.dec);
            }
            return message;
        }
        private int F_GetFeedRate()
        {
            Focas1.ODBACT feedrate = new Focas1.ODBACT();
            ret = Focas1.cnc_actf(h, feedrate);
            return feedrate.data;
        }
        private int F_GetSpindleSpeed()
        {
            Focas1.ODBSPEED speed = new Focas1.ODBSPEED();
            ret = Focas1.cnc_rdspeed(h, 1, speed);
            return speed.acts.data;
        }
        private string F_ReadVar(short n_variabile)
        {
            short ldati = 10;
            Focas1.ODBM variabile = new Focas1.ODBM();
            ret = Focas1.cnc_rdmacro(h, n_variabile, ldati, variabile);
            string message = String.Empty;
            if (ret == 0)
            {
                if (variabile.mcr_val == 0 && variabile.dec_val == -1)
                { }
                else
                {
                    message += (variabile.mcr_val / Math.Pow(10, variabile.dec_val)).ToString();
                }
            }
            return message;
        }
        private string[] F_ReadVars(int start_point, int num_points)
        {
            double[] result = new double[num_points];
            ret = Focas1.cnc_rdmacror2(h, start_point, ref num_points, result);
            string[] message = new string[num_points];
            for (int i = 0; i < message.Length; i++)
                message[i] = string.Empty;
            if (ret == 0)
            {
                for (int i = 0; i < num_points; i++)
                    message[i] = result[i] + "";
            }
            return message;
        }
        private bool F_WriteVar(int start_point, double[] forced_vars)
        {
            int num_points = forced_vars.Length;
            ret = Focas1.cnc_wrmacror2(h, start_point, ref num_points, forced_vars);
            return ret == Focas1.EW_OK;
        }
        private short F_ReadParamInfo(short param_number, ushort how_many)
        {
            Focas1.ODBPARAIF paraminfo = new Focas1.ODBPARAIF();
            ret = Focas1.cnc_rdparainfo(h, param_number, how_many, paraminfo);
            return paraminfo.info.info1.prm_type;
        }
        private string F_ReadParamInfoBin(short param_number, ushort how_many)
        {
            Focas1.ODBPARAIF paraminfo = new Focas1.ODBPARAIF();
            ret = Focas1.cnc_rdparainfo(h, param_number, how_many, paraminfo);
            return Convert.ToString(paraminfo.info.info1.prm_type, 2);
        }
        private string F_ReadParma(short det, short param_number, short ctrlAxis)
        {
            string message = String.Empty;
            short n_axis = -1;
            int ldati = 4 + det * Focas1.MAX_AXIS;
            Focas1.IODBPSD param = new Focas1.IODBPSD();
            ret = Focas1.cnc_rdparam(h, param_number, n_axis, (short)ldati, param);
            if (ret == 0)
            {
                for (int i = 0; i < ctrlAxis; i++)
                    message += "parametro " + param_number.ToString() + " asse " + i.ToString() + ": " + param.u.cdatas.ElementAt(i).ToString() + "\n";
            }
            else
                message = "Errore";
            return message;
        }
        private void F_WriteParam(short datano, short n_axis, int ldata)
        {
            Focas1.IODBPSD paramx = new Focas1.IODBPSD();
            paramx.datano = datano;
            paramx.type = n_axis;
            paramx.u.ldata = ldata;
            ret = Focas1.cnc_wrparam(h, 4 + 4 * 1, paramx);
        }
        private void F_WriteParam(short datano, short n_axis, byte cdata)
        {
            //MessageBox.Show("Not Implemented Yet");
            Focas1.IODBPSD paramx = new Focas1.IODBPSD();
            paramx.datano = datano;
            paramx.type = n_axis;
            paramx.u.cdata = cdata;
            ret = Focas1.cnc_wrparam(h, 4 + 1 * 1, paramx);
        }
        private void F_WriteParam(short datano, short n_axis, short idata)
        {
            //MessageBox.Show("Not Implemented Yet");
            Focas1.IODBPSD paramx = new Focas1.IODBPSD();
            paramx.datano = datano;
            paramx.type = n_axis;
            paramx.u.idata = idata;
            ret = Focas1.cnc_wrparam(h, 4 + 2 * 1, paramx);
        }
        private void F_WriteParam(short datano, short n_axis, Focas1.REALPRM rdata)
        {
            MessageBox.Show("Not Implemented Yet");
            return;
            Focas1.IODBPSD paramx = new Focas1.IODBPSD();
            paramx.datano = datano;
            paramx.type = n_axis;
            paramx.u.rdata = rdata;
            ret = Focas1.cnc_wrparam(h, 4 + 4 * 1, paramx);
        }
        private void F_WriteParams(short datano, short n_axis, int[] ldatas)
        {
            Focas1.IODBPSD paramx = new Focas1.IODBPSD();
            paramx.datano = datano;
            paramx.type = n_axis;
            for (int i = 0; i < ldatas.Length; i++)
                paramx.u.ldatas[i] = ldatas[i];

            ret = Focas1.cnc_wrparam(h, 4 + 4 * 1, paramx);
        }
        private void F_WriteParams(short datano, short n_axis, byte[] cdatas)
        {
            Focas1.IODBPSD paramx = new Focas1.IODBPSD();
            paramx.datano = datano;
            paramx.type = n_axis;
            for (int i = 0; i < cdatas.Length; i++)
                paramx.u.cdatas[i] = cdatas[i];
            ret = Focas1.cnc_wrparam(h, 4 + 1 * 1, paramx);
        }
        private void F_WriteParams(short datano, short n_axis, short[] idatas)
        {
            Focas1.IODBPSD paramx = new Focas1.IODBPSD();
            paramx.datano = datano;
            paramx.type = n_axis;
            for (int i = 0; i < idatas.Length; i++)
                paramx.u.idatas[i] = idatas[i];
            ret = Focas1.cnc_wrparam(h, 4 + 2 * 1, paramx);
        }
        private void F_WriteParams(short datano, short n_axis, Focas1.REALPRM[] rdatas)
        {
            MessageBox.Show("Not Implemented Yet");
            return;
            Focas1.IODBPSD paramx = new Focas1.IODBPSD();
            paramx.datano = datano;
            paramx.type = n_axis;
            for (int i = 0; i < rdatas.Length; i++)
                paramx.u.rdatas[i] = rdatas[i];
            ret = Focas1.cnc_wrparam(h, 4 + 1 * 1, paramx);
        }
        private void F_WriteParamRec()
        {
            MessageBox.Show("Not Implemented Yet");
            return;
        }
        private void F_WriteParam_ext()
        {
            MessageBox.Show("Not Implemented Yet");
            return;
        }
        private int F_GetActivePMC()
        {
            int npmc = 0;
            ret = Focas1.pmc_get_current_pmc_unit(h, ref npmc);
            return npmc;
        }
        private string F_ReadPMCArea(short adr_type, short data_type, ushort start, ushort end)
        {

            int lenght = (int)end - (int)start;
            string message = String.Empty;
            switch (data_type)
            {
                case 0: //Byte
                    Focas1.IODBPMC0 pmcdata0 = new Focas1.IODBPMC0();
                    ret = Focas1.pmc_rdpmcrng(h, adr_type, data_type, start, end, (ushort)(8 + 1 * lenght), pmcdata0);
                    if (ret == 0)
                    {
                        for (int i = 0; i < lenght; i++)
                            message += "Area PMC n" + i.ToString() + ": " + pmcdata0.cdata.GetValue(i).ToString();
                    }
                    break;
                case 1: //Word type
                    Focas1.IODBPMC1 pmcdata1 = new Focas1.IODBPMC1();
                    ret = Focas1.pmc_rdpmcrng(h, adr_type, data_type, start, end, (ushort)(8 + 2 * lenght), pmcdata1);
                    if (ret == 0)
                    {
                        for (int i = 0; i < lenght; i++)
                            message += "Area PMC n" + i.ToString() + ": " + pmcdata1.idata.GetValue(i).ToString();
                    }
                    break;
                case 2:
                    Focas1.IODBPMC2 pmcdata2 = new Focas1.IODBPMC2();

                    ret = Focas1.pmc_rdpmcrng(h, adr_type, data_type, start, end, (ushort)(8 + 4 * lenght), pmcdata2);
                    if (ret == 0)
                    {
                        for (int i = 0; i < lenght; i++)
                            message += "Area PMC n" + i.ToString() + ": " + pmcdata2.ldata.GetValue(i).ToString();
                    }
                    break;
                case 4:
                    Focas1.IODBPMC4 pmcdata4 = new Focas1.IODBPMC4();

                    ret = Focas1.pmc_rdpmcrng(h, adr_type, data_type, start, end, (ushort)(8 + 4 * lenght), pmcdata4);
                    if (ret == 0)
                    {
                        for (int i = 0; i < lenght; i++)
                            message += "Area PMC n" + i.ToString() + ": " + pmcdata4.fdata.GetValue(i).ToString();
                    }
                    break;
                case 5:
                    Focas1.IODBPMC5 pmcdata5 = new Focas1.IODBPMC5();

                    ret = Focas1.pmc_rdpmcrng(h, adr_type, data_type, start, end, (ushort)(8 + 8 * lenght), pmcdata5);
                    if (ret == 0)
                    {
                        for (int i = 0; i < lenght; i++)
                            message += "Area PMC n" + i.ToString() + ": " + pmcdata5.ddata.GetValue(i).ToString();
                    }
                    break;
                default:
                    break;
            }
            return message;
        }
        private bool F_WritePMCArea0(short data_type, short start, byte[] data)
        {
            int end = start + data.Length;
            Focas1.IODBPMC0 pmcdata = new Focas1.IODBPMC0();
            pmcdata.type_a = 0;
            pmcdata.type_d = data_type;
            pmcdata.datano_s = (short)start;
            pmcdata.datano_e = (short)end;
            ushort length_bytes = (ushort)(8 + data.Length);
            for (int i = 0; i < data.Length + 1; i++)
            {
                pmcdata.cdata[i] = data[i];
            }
            ret = Focas1.pmc_wrpmcrng(h, length_bytes, pmcdata);
            return ret == Focas1.EW_OK;
        }
        private bool F_WritePMCArea1(short data_type, short start, short[] data)
        {
            int end = start + data.Length;
            Focas1.IODBPMC1 pmcdata = new Focas1.IODBPMC1();
            pmcdata.type_a = 1;
            pmcdata.type_d = data_type;
            pmcdata.datano_s = (short)start;
            pmcdata.datano_e = (short)end;
            ushort length_bytes = (ushort)(8 + data.Length);
            for (int i = 0; i < data.Length + 1; i++)
            {
                pmcdata.idata[i] = data[i];
            }
            ret = Focas1.pmc_wrpmcrng(h, length_bytes, pmcdata);
            return ret == Focas1.EW_OK;
        }
        private bool F_WritePMCArea2(short data_type, short start, int[] data)
        {
            int end = start + data.Length;
            Focas1.IODBPMC2 pmcdata = new Focas1.IODBPMC2();
            pmcdata.type_a = 2;
            pmcdata.type_d = data_type;
            pmcdata.datano_s = (short)start;
            pmcdata.datano_e = (short)end;
            ushort length_bytes = (ushort)(8 + data.Length);
            for (int i = 0; i < data.Length + 1; i++)
            {
                pmcdata.ldata[i] = data[i];
            }
            ret = Focas1.pmc_wrpmcrng(h, length_bytes, pmcdata);
            return ret == Focas1.EW_OK;
        }
        private bool F_WritePMCArea4(short data_type, short start, float[] data)
        {
            int end = start + data.Length;
            Focas1.IODBPMC4 pmcdata = new Focas1.IODBPMC4();
            pmcdata.type_a = 4;
            pmcdata.type_d = data_type;
            pmcdata.datano_s = (short)start;
            pmcdata.datano_e = (short)end;
            ushort length_bytes = (ushort)(8 + data.Length);
            for (int i = 0; i < data.Length + 1; i++)
            {
                pmcdata.fdata[i] = data[i];
            }
            ret = Focas1.pmc_wrpmcrng(h, length_bytes, pmcdata);
            return ret == Focas1.EW_OK;
        }
        private bool F_WritePMCArea5(short data_type, short start, double[] data)
        {
            int end = start + data.Length;
            Focas1.IODBPMC5 pmcdata = new Focas1.IODBPMC5();
            pmcdata.type_a = 5;
            pmcdata.type_d = data_type;
            pmcdata.datano_s = (short)start;
            pmcdata.datano_e = (short)end;
            ushort length_bytes = (ushort)(8 + data.Length);
            for (int i = 0; i < data.Length + 1; i++)
            {
                pmcdata.ddata[i] = data[i];
            }
            ret = Focas1.pmc_wrpmcrng(h, length_bytes, pmcdata);
            return ret == Focas1.EW_OK;
        }
        private string F_GetAlarms()
        {
            short ccc = 1;
            Focas1.ODBPMCALM pmcalm = new Focas1.ODBPMCALM();
            //unsigned short FlibHndl, short s_number, short *read_num, short *exist, ODBPMCALM *pmcalm
            ret = Focas1.pmc_rdalmmsg(h, 1, ref ccc, out short exist, pmcalm);
            if (exist >= 0)
                return pmcalm.msg1.almmsg;
            else
                return "";
        }
        private string F_GetAlarms2()
        {
            int codice_allarme = 0;
            ret = Focas1.cnc_alarm2(h, out codice_allarme);
            string message = String.Empty;
            if (ret == 0)
            {
                message = codice_allarme.ToString();
            }
            return message;
        }
        private string F_GetErrMessage()
        {
            Focas1.ODBALMMSG2 allarmessage = new Focas1.ODBALMMSG2();
            short nummessages = 1;
            string message = String.Empty;
            ret = Focas1.cnc_rdalmmsg2(h, -1, ref nummessages, allarmessage);
            if (ret == 0)
            {
                message += "Alarm: " + allarmessage.msg1.alm_no.ToString() + " type: " + allarmessage.msg1.type.ToString() + " message: " + allarmessage.msg1.alm_msg;
            }
            return message;
        }
        private StatInfo F_GetStatusInfo()
        {
            Focas1.ODBST machine_status = new Focas1.ODBST();
            ret = Focas1.cnc_statinfo(h, machine_status);
            StatInfo machine_state = new StatInfo { };
            machine_state.tmmode = 0;
            machine_state.aut = 0;
            machine_state.run = 0;
            machine_state.motion = 0;
            machine_state.mstb = 0;
            machine_state.emergency = 0;
            machine_state.alarm = 0;
            machine_state.edit = 0;

            if (ret == 0)
            {
                machine_state.tmmode = machine_status.tmmode;
                machine_state.aut = machine_status.aut;
                machine_state.run = machine_status.run;
                machine_state.motion = machine_status.motion;
                machine_state.mstb = machine_status.mstb;
                machine_state.emergency = machine_status.emergency;
                machine_state.alarm = machine_status.alarm;
                machine_state.edit = machine_status.edit;
            }
            return machine_state;
        }
        private StatInfo2 F_GetStatusInfo2()
        {
            Focas1.ODBST2 machine_status2 = new Focas1.ODBST2();
            ret = Focas1.cnc_statinfo2(h, machine_status2);
            StatInfo2 machine_status = new StatInfo2 { };
            machine_status.warning = 0;
            machine_status.o3dchk = 0;
            machine_status.ext_opt = 0;
            machine_status.restart = 0;

            if (ret == 0)
            {
                machine_status.warning = machine_status2.warning;
                machine_status.o3dchk = machine_status2.o3dchk;
                machine_status.ext_opt = machine_status2.ext_opt;
                machine_status.restart = machine_status2.restart;
            }
            return machine_status;
        }
        private string F_CNCDiagnoss(short n_diagnosis, short n_axis, short dsize)
        {
            Focas1.ODBDGN diags = new Focas1.ODBDGN();
            int lenght = 4 + (dsize) * (n_axis);
            ret = Focas1.cnc_diagnoss(h, n_diagnosis, n_axis, (short)lenght, diags);
            if (ret == 0)
                return diags.u.cdata.ToString();
            return "";
        }
        private string F_GetExtDiag2(short n_diagn, int[] val)
        {
            string[] messages = new string[n_diagn];
            for (int i = 0; i < n_diagn; i++)
                messages[i] = String.Empty;

            if (n_diagn != val.Length) return "";

            Focas1.IODBPRM2 prm = new Focas1.IODBPRM2();
            int[] prmno = new int[n_diagn];
            byte[] bytes2 = new byte[Marshal.SizeOf(prm) * n_diagn];
            IntPtr ptrWork2 = Marshal.AllocCoTaskMem(Marshal.SizeOf(prm));
            for (int i = 0; i < val.Length; i++)
                prmno[i] = val[i];

            ret = Focas1.cnc_rddiag_ext(h, prmno, n_diagn, bytes2);
            if (ret == Focas1.EW_OK)
            {
                for (int pos = 0; pos < Marshal.SizeOf(prm) * n_diagn; pos += Marshal.SizeOf(prm))
                {
                    Marshal.Copy(bytes2, pos, ptrWork2, Marshal.SizeOf(prm));
                    Marshal.PtrToStructure(ptrWork2, prm);
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data1.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data2.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data3.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data4.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data5.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data6.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data7.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data8.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data9.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data10.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data11.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data12.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data13.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data14.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data15.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data16.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data17.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data18.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data19.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data20.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data21.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data22.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data23.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data24.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data25.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data26.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data27.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data28.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data29.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data30.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data31.prm_val.ToString() + "\n";
                    messages[pos / Marshal.SizeOf(prm)] += prm.data.data32.prm_val.ToString() + "\n";

                }

            }
            Marshal.FreeCoTaskMem(ptrWork2);
            string returnMSG = String.Join("\n", messages);
            return returnMSG;
        }
        private void F_DownloadPRG(string strPrg)
        {
            int len, n;

            ret = Focas1.cnc_dwnstart4(h, 0, "//CNC_MEM/USER/PATH1");
            if (ret != Focas1.EW_OK)
            {
                Console.WriteLine("FAILED");
                Console.ReadLine();
            }


            int startPos = 0;
            len = strPrg.Length;
            Console.WriteLine("LEN=" + len);

            while (len > 0)
            {
                char[] prg = new char[1280];
                strPrg.CopyTo(startPos, prg, 0, len);

                n = len;
                ret = Focas1.cnc_download4(h, ref n, prg);
                if (ret == (short)Focas1.focas_ret.EW_BUFFER)
                {
                    continue;
                }
                if (ret == Focas1.EW_OK)
                {
                    startPos += n;
                    len -= n;
                }
                if (ret != Focas1.EW_OK)
                {
                    Console.WriteLine("PROBLEM IN DOWNLOAD4 CORE=" + ret);
                    break;
                }
            }
            ret = Focas1.cnc_dwnend4(h);
            Console.WriteLine("PROBLEM IN DOWNLOAD4 END=" + ret);
        }
        private string F_Upload_PRG()
        {
            /**************** cnc_upload4 ****************/
            Console.WriteLine("\n----cnc_upload4----");
            char[] buf = new char[1280];
            string result = string.Empty;
            ret = Focas1.cnc_upstart4(h, 0, "//CNC_MEM/USER/PATH1/PROG123");
            if (ret != Focas1.EW_OK)
            {
                return "-1";
            }
            else
            {
                do
                {
                    int len = 1280;
                    ret = Focas1.cnc_upload4(h, ref len, buf);
                    if (ret == (short)Focas1.focas_ret.EW_BUFFER)
                    {
                        continue;
                    }
                    if (ret == (short)Focas1.focas_ret.EW_OK)
                    {
                        buf[len] = '\0';
                        result = new string(buf);
                    }
                    if (buf[len - 1] == '%')
                    {
                        break;
                    }
                } while ((ret == Focas1.EW_OK) || (ret == (short)Focas1.focas_ret.EW_BUFFER));
            }
            ret = Focas1.cnc_upend4(h);
            return result;
        }
        private string F_GetMainPRG()
        {
            char[] chars = new char[1281];
            ret = Focas1.cnc_pdf_rdmain(h, chars);
            string charsStr = new string(chars);
            if (ret == Focas1.EW_OK)
                return charsStr.ToString();
            else return "Failed Download";
        }
        private bool F_AddFolder(string path)
        {
            ret = Focas1.cnc_pdf_add(h, path);
            return ret == Focas1.EW_OK;
        }
        private bool F_DelFolder(string path)
        {
            ret = Focas1.cnc_pdf_del(h, path);
            return ret == Focas1.EW_OK;
        }
        private string F_ActiveTool( short type, short num_cmd)
        {
            Focas1.ODBCMD command = new Focas1.ODBCMD();
            ret = Focas1.cnc_rdcommand(h, type, 1, ref num_cmd, command);
            if (ret == 0)
                return command.cmd0.cmd_val.ToString();
            return "";
        }
        private void F_CloseConn()
        {
            Focas1.cnc_freelibhndl(h);
        }
        #endregion
        #region Form
        SystemInfo info;
        SystemInfoEX infoEx;
        StatInfo statInfo;
        StatInfo2 statInfo2;
        private void getActivePath_Click(object sender, EventArgs e)
        {
            this.activePath.Text = this.F_GetActivePath();
            this.maxPath.Text = this.F_GetMaxPath();
        }
        private void setPathButton_Click(object sender, EventArgs e)
        {
            string vIn = this.setPath.Text;
            try
            {
                short vOut = Convert.ToInt16(vIn);
                this.F_SetPath(vOut);
            }
            catch
            {
                MessageBox.Show("Path must be a number", "ERROR!");
            }

        }

        private void getSysInfoEx_Click(object sender, EventArgs e)
        {
        infoEx = this.F_GetSysInfoEx();
        }
        private void getSysInfo_Click(object sender, EventArgs e)
        {
        info = this.F_GetSysInfo();
        }
        private void getAbsAxis_Click(object sender, EventArgs e)
        {
            this.absAxis.Text = this.F_GetAbsAxis(infoEx.ctrl_axis);
        }

        private void getCoord_Click(object sender, EventArgs e)
        {
            this.coord.Text = this.F_GetCoord(infoEx.ctrl_axis);
        }

        private void getCoord1_Click(object sender, EventArgs e)
        {
            this.coord1.Text = this.F_GetCoord1(infoEx.ctrl_axis);
        }

        private void GetFeedRate_Click(object sender, EventArgs e)
        {
            this.FeedRate.Text = this.F_GetFeedRate().ToString();
        }

        private void GetSpeed_Click(object sender, EventArgs e)
        {
            this.SpindleSpeed.Text = this.F_GetSpindleSpeed().ToString();
        }

        private void ReadVar_Click(object sender, EventArgs e)
        {
            try
            {
                short var = Int16.Parse(this.Var2F.Text);
                this.VarRead.Text = this.F_ReadVar(var);
            }
            catch
            {
                MessageBox.Show("Variable Must be a number", "ERROR!");
            }
        }

        private void WriteVar_Click(object sender, EventArgs e)
        {
            List<double> forcedVars = new List<double>();
            try {
                forcedVars.Add(Double.Parse(this.W_V_Value.Text));

                int start = int.Parse(this.W_V_Addr.Text);
                bool writeOk = this.F_WriteVar(start, forcedVars.ToArray());
                this.W_V_Status.Text = writeOk ? "OK" : "Failed";
            }
            catch
            {
                MessageBox.Show("Value must be a number", "ERROR!");
            }
        }
        short dat;
        private void ReadParam_Click(object sender, EventArgs e)
        {
            try
            {
                short param = Int16.Parse(this.paramNum.Text);
                dat = this.F_ReadParamInfo(param, 1);
                this.PInfo.Text = dat.ToString();
            }
            catch
            {
                MessageBox.Show("Paramiter must be a number", "ERROR!");
            }
        }

        private void ReadParamBin_Click(object sender, EventArgs e)
        {
            try
            {
                short param = Int16.Parse(this.paramBNum.Text);
                this.PBInfo.Text = this.F_ReadParamInfoBin(param, 1).ToString();
            }
            catch
            {
                MessageBox.Show("Paramiter must be a number", "ERROR!");
            }
        }

        private void ReadParma_Click(object sender, EventArgs e)
        {
            string dimAssi = Convert.ToString(dat, 2);
            short det = Int16.Parse(dimAssi);
            try {
                short param = Int16.Parse(this.Parma.Text);
                this.outParma.Text = this.F_ReadParma(det, param, infoEx.ctrl_axis);
            }
            catch
            {
                MessageBox.Show("Paramiter must be a number", "ERROR!");
            }
        }
        private void GetActivePmc_Click(object sender, EventArgs e)
        {
            this.ActivePmc.Text = this.F_GetActivePMC().ToString();
        }

        private void ReadPMCArea_Click(object sender, EventArgs e)
        {
            short addr_type = addrType(this.PMCAdrType.Text);
            short data_type = dataType(this.PMCDataType.Text);
            if (data_type == -1) return;
            if (addr_type == -1) return;
            try {
                int start = Int16.Parse(this.PMCStart.Text);
                int stop = start + 1;
                this.PMCReturn.Text = this.F_ReadPMCArea(addr_type, data_type, (ushort)start, (ushort)stop);
            }
            catch
            {
                MessageBox.Show("Value must be a number");
            }
        }

        short addrType(string t) { 
            switch (t) {
                case "G": return 0;
                case "F": return 1;
                case "Y": return 2;
                case "X": return 3;
                case "A": return 4;
                case "R": return 5;
                case "T": return 6;
                case "K": return 7;
                case "C": return 8;
                case "D": return 9;
                case "M": return 10;
                case "N": return 11;
                case "E": return 12;
                case "Z": return 13;
                default:  return -1;
            } 
        } 
        short dataType(string t)
        {
            if (t == "Byte") return 0;
            if (t == "Word") return 1;
            if (t == "Long") return 2;
            if (t == "64 bit Float") return 5;
            if (t == "32 bit Float") return 4;
            return -1;
        }
        private void GetAlarms_Click(object sender, EventArgs e)
        {
            this.Alarms.Text = this.F_GetAlarms();
        }

        private void GetAlarms2_Click(object sender, EventArgs e)
        {
            this.Alarms2.Text = this.F_GetAlarms2();
        }

        private void GetStatusInfo_Click(object sender, EventArgs e)
        {
            statInfo = this.F_GetStatusInfo();
        }


        #endregion

        private void GetStatusInfo2_Click(object sender, EventArgs e)
        {
            statInfo2 = this.F_GetStatusInfo2();
        }

        private void CNCDiagnosis_Click(object sender, EventArgs e)
        {
            try {
                short numD = Int16.Parse(this.NDiag.Text);
                short axis = infoEx.max_axis;
                short size = Int16.Parse(this.DiagSize.Text);
                int dSize = 4 + size * axis;
                this.DiagOut.Text= this.F_CNCDiagnoss(numD, axis,(short)dSize );
            }
            catch {
                MessageBox.Show("Numero di Diagnistica deve essere un numero", "ERRORE!");

            }
        }

        private void CNCDiagExtra_Click(object sender, EventArgs e)
        {
            try
            {
                int val1 = Int16.Parse(this.DiagV1.Text);
                int val2 = Int16.Parse(this.DiagV2.Text);
                int val3 = Int16.Parse(this.DiagV3.Text);
                short nDia = Int16.Parse(this.DiagN.Text);
                int[] val = new int[] { val1, val2, val3 };
                this.DExtOut.Text = this.F_GetExtDiag2(nDia, val);
            }
            catch
            {
                MessageBox.Show("Value format error all value must be a number", "ERRORE!");
            }
        }

        private void UploadProg_Click(object sender, EventArgs e)
        {
            this.PRG.Text= this.F_Upload_PRG();
        }

        private void ActProgButton_Click(object sender, EventArgs e)
        {
            this.ActProg.Text = this.F_GetMainPRG();
        }

        private void GetTool_Click(object sender, EventArgs e)
        {
            try
            {
                short type = Int16.Parse(this.Type.Text);
                short prts = Int16.Parse(this.Point.Text);
                this.Tool.Text = this.F_ActiveTool(type, prts);
            }
            catch
            {
                MessageBox.Show("Some value must be a number", "ERRORE!");
            }
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.F_CloseConn();
            this.Close();
        }
    }

}
