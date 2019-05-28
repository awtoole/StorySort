import React, { Component } from 'react';
import 'dropzone';
import '../../node_modules/dropzone/dist/basic.css';
import '../../node_modules/dropzone/dist/dropzone.css'; 


export class FileDragDrop extends Component {
    render() {
        return (
            <form action="api/SampleData/FileUpload" className="dropzone">
                <div className="fallback">
                    <input name="file" type="file" />
                </div>
            </form>
        );
    }
}