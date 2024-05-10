/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package javafxapplication3;

import javafx.application.Application;
import javafx.geometry.Pos;
import javafx.scene.Scene;
import javafx.stage.Stage;

/**
 *
 * @author Brady Gehrman
 */
public class JavaFXApplication5 extends Application{

    @Override
    public void start(Stage primaryStage) throws Exception {
        OutputPane pane = new OutputPane();
        pane.setAlignment(Pos.CENTER);
        Scene scene = new Scene(pane, 500, 250);
        //add the pane to the scene
        primaryStage.setTitle("Assignment 5");
        primaryStage.setScene(scene);
        primaryStage.show();
        
        
        
    }
    
     public static void main(String[] args) {
        launch(args);
    }
}
