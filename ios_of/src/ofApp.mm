#include "ofApp.h"

//--------------------------------------------------------------
void ofApp::setup(){
    
    // initialize visualization propeties
    OFPlayerColor.set(255, 0, 0);       // red
    unityPlayerColor.set(0, 255, 0);    // green
    playerRadius = 42;
    
    OFPlayerPosition.set(0, 0);
    unityPlayerPosition.set(0,0);
    
    
    // initialize OSC handling
    //
    
    // open an outgoing connection to HOST:PORT
    sender.setup( OSC_OUT_HOST, OSC_OUT_PORT );
    
    // listen on the given port
    receiver.setup( OSC_INC_PORT );
}

//--------------------------------------------------------------
void ofApp::update(){
    
    // OSC: SENDING (OUT)
    //
    
    // heartbeat on iOS as the phone will shut down the network connection to save power
    // this keeps the network alive as it thinks it is being used.
    if( ofGetFrameNum() % 120 == 0 ){
        ofxOscMessage m;
        m.setAddress( OF_heartbeat );
        m.addIntArg( ofGetFrameNum() );
        sender.sendMessage( m );
    }

    
    // OSC: RECEIVING (INC)
    //
    
    // check for waiting messages
    while( receiver.hasWaitingMessages() ){
        
        // get the next message
        ofxOscMessage m;
        receiver.getNextMessage(m);
        
        // react to incoming messages according to their OSC address
        //
        
        // update position of Unity Player Controller
        if(m.getAddress() == unity_update_position){
            
            // receive values from message
            // normalization to larger iOS units: multiply by 100.0f
            // Note: Unity z coordinate == iOS y coordinate
            float x = m.getArgAsFloat(0) * 100.0f;
            float y = m.getArgAsFloat(2) * 100.0f; // (OF y == Unity z)
            
            unityPlayerPosition.set(x, y);
        }
    }
}

//--------------------------------------------------------------
void ofApp::draw(){
    
    // draw player controllers
    drawOFPlayerController();
    drawUnityPlayerController();
    
    // display debug info
    drawDebugInfo();
}

//--------------------------------------------------------------
void ofApp::exit(){

}

//--------------------------------------------------------------
void ofApp::touchDown(ofTouchEventArgs & touch){

}

//--------------------------------------------------------------
void ofApp::touchMoved(ofTouchEventArgs & touch){

    // update openFrameworks Player Controller
    OFPlayerPosition.set(touch.x, touch.y);
    

    // OSC: SENDING (OUT)
    //
    
    // create new outgoing message to update openFrameworks Player Controller
    ofxOscMessage m;
    m.setAddress( OF_update_position );
    
    // set values attached to the outgoing message
    m.addIntArg( touch.x );
    m.addIntArg( touch.y );
    
    // send message
    sender.sendMessage( m );
}

//--------------------------------------------------------------
void ofApp::touchUp(ofTouchEventArgs & touch){

}

//--------------------------------------------------------------
void ofApp::touchDoubleTap(ofTouchEventArgs & touch){

}

//--------------------------------------------------------------
void ofApp::touchCancelled(ofTouchEventArgs & touch){
    
}

//--------------------------------------------------------------
void ofApp::lostFocus(){

}

//--------------------------------------------------------------
void ofApp::gotFocus(){

}

//--------------------------------------------------------------
void ofApp::gotMemoryWarning(){

}

//--------------------------------------------------------------
void ofApp::deviceOrientationChanged(int newOrientation){

}

//--------------------------------------------------------------
void ofApp::drawOFPlayerController(){
    ofSetColor(OFPlayerColor);
    ofFill();
    ofDrawCircle(OFPlayerPosition, playerRadius);
}

//--------------------------------------------------------------
void ofApp::drawUnityPlayerController(){
    ofSetColor(unityPlayerColor);
    ofFill();
    ofDrawCircle(unityPlayerPosition, playerRadius);
}

//--------------------------------------------------------------
void ofApp::drawDebugInfo(){
    
    // draw some application feedback
    //
    
    // set color: OF pink
    ofSetColor(236, 50, 135);
    
    // draw the framerate (FPS, "frames per second")
    ofDrawBitmapString(ofToString(ofGetFrameRate()) + "fps", 10, 15);
    
    // draw the touch event's position
    ofDrawBitmapString("Touch x|y: " + ofToString(OFPlayerPosition.x) + "|" + ofToString(OFPlayerPosition.y), 110, 15);
}

