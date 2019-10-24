﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharpPro;
//using Crestron.SimplSharpPro.DeviceSupport;
using Crestron.SimplSharpPro.DM;
using Crestron.SimplSharpPro.DM.Endpoints;
using Crestron.SimplSharpPro.DM.Endpoints.Transmitters;

using PepperDash.Core;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.DM.Config;

namespace PepperDash.Essentials.DM
{
    using eVst = Crestron.SimplSharpPro.DeviceSupport.eX02VideoSourceType;
    using eAst = Crestron.SimplSharpPro.DeviceSupport.eX02AudioSourceType;

    public class DmTx4k100Controller : BasicDmTxControllerBase, IRoutingInputsOutputs, IHasFeedback,
        IIROutputPorts, IComPorts, ICec
    {
        public DmTx4K100C1G Tx { get; private set; }

        public RoutingInputPort HdmiIn { get; private set; }
        public RoutingOutputPort DmOut { get; private set; }

        //public IntFeedback VideoSourceNumericFeedback { get; protected set; }
        //public IntFeedback AudioSourceNumericFeedback { get; protected set; }
        //public IntFeedback HdmiIn1HdcpCapabilityFeedback { get; protected set; }
        //public IntFeedback HdmiIn2HdcpCapabilityFeedback { get; protected set; }

        //public override IntFeedback HdcpSupportAllFeedback { get; protected set; }
        //public override ushort HdcpSupportCapability { get; protected set; }

        /// <summary>
        /// Helps get the "real" inputs, including when in Auto
        /// </summary>
        public Crestron.SimplSharpPro.DeviceSupport.eX02VideoSourceType ActualActiveVideoInput
        {
            get
                {
                    return eVst.Hdmi1;
                }
        }
        public RoutingPortCollection<RoutingInputPort> InputPorts
        {
            get
            {
                return new RoutingPortCollection<RoutingInputPort> 
				{ 
					HdmiIn				 
				};
            }
        }
        public RoutingPortCollection<RoutingOutputPort> OutputPorts
        {
            get
            {
                return new RoutingPortCollection<RoutingOutputPort> { DmOut };
            }
        }
        public DmTx4k100Controller(string key, string name, DmTx4K100C1G tx)
            : base(key, name, tx)
        {
            Tx = tx;

            HdmiIn = new RoutingInputPort(DmPortName.HdmiIn1,
                eRoutingSignalType.Audio | eRoutingSignalType.Video, eRoutingPortConnectionType.Hdmi, eVst.Hdmi1, this);

            DmOut = new RoutingOutputPort(DmPortName.DmOut, eRoutingSignalType.Audio | eRoutingSignalType.Video,
                eRoutingPortConnectionType.DmCat, null, this);

            // Set Ports for CEC
            HdmiIn.Port = Tx;
        }



        public override bool CustomActivate()
        {
            // Link up all of these damned events to the various RoutingPorts via a helper handler


            // Base does register and sets up comm monitoring.
            return base.CustomActivate();
        }


        #region IIROutputPorts Members
        public CrestronCollection<IROutputPort> IROutputPorts { get { return Tx.IROutputPorts; } }
        public int NumberOfIROutputPorts { get { return Tx.NumberOfIROutputPorts; } }
        #endregion

        #region IComPorts Members
        public CrestronCollection<ComPort> ComPorts { get { return Tx.ComPorts; } }
        public int NumberOfComPorts { get { return Tx.NumberOfComPorts; } }
        #endregion

        #region ICec Members
        public Cec StreamCec { get { return Tx.StreamCec; } }
        #endregion
    }
}