#pragma once

#include "ofxiOS.h"
#include "ofxOsc.h"

#define OSC_OUT_HOST "10.0.1.6"
#define OSC_OUT_PORT 30001  // == Unity incoming port
#define OSC_INC_PORT 30002  // == Unity outgoing port

class ofApp : public ofxiOSApp {
	
    public:
        void setup();
        void update();
        void draw();
        void exit();
	
        void touchDown(ofTouchEventArgs & touch);
        void touchMoved(ofTouchEventArgs & touch);
        void touchUp(ofTouchEventArgs & touch);
        void touchDoubleTap(ofTouchEventArgs & touch);
        void touchCancelled(ofTouchEventArgs & touch);

        void lostFocus();
        void gotFocus();
        void gotMemoryWarning();
        void deviceOrientationChanged(int newOrientation);
    
        // visualization attributes for representing the manipulated objects
        ofColor OFPlayerColor;
        ofColor unityPlayerColor;
        int playerRadius;
    
        // keeping track of the player positions
        ofPoint OFPlayerPosition;
        ofPoint unityPlayerPosition;
    
        // draw the player representations
        void drawOFPlayerController();
        void drawUnityPlayerController();
    
        // display some debugging information
        void drawDebugInfo();
    
        // OSC handling
        ofxOscSender sender;        // hanlding outgoing messages
        ofxOscReceiver receiver;    // handling incoming messages
    
        // list of defined OSC addresses
        std::string OF_update_position = "/openFrameworks/position";
        std::string OF_heartbeat = "/misc/heartbeat";
        std::string unity_update_position = "/unity/position";
};


